using System.Data;
using System.Globalization;
using EnergyDataUploader.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace EnergyDataUploader.Repositories
{
    public class UploadRepository : IUploadRepository
    {
        private readonly EnergyDbContext _context;

        public UploadRepository(EnergyDbContext context)
        {
            _context = context;
        }

        public async Task<string> ProcessExcelUploadAsync(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            });

            var consumoTable = result.Tables[0];
            var costoTable = result.Tables[1];
            var perdidasTable = result.Tables[2];

            consumoTable.PrimaryKey = new DataColumn[] { consumoTable.Columns["Linea"], consumoTable.Columns["Fecha"] };
            costoTable.PrimaryKey = new DataColumn[] { costoTable.Columns["Linea"], costoTable.Columns["Fecha"] };
            perdidasTable.PrimaryKey = new DataColumn[] { perdidasTable.Columns["Linea"], perdidasTable.Columns["Fecha"] };

            var tramosCache = new Dictionary<string, DimTramo>();
            var fechasCache = new Dictionary<int, DimFecha>();

            foreach (DataRow consumoRow in consumoTable.Rows)
            {
                if (consumoRow["Linea"] == DBNull.Value || string.IsNullOrWhiteSpace(consumoRow["Linea"].ToString()))
                    continue;

                var linea = consumoRow["Linea"].ToString().Trim();
                var fecha = DateTime.Parse(consumoRow["Fecha"].ToString());
                var fechaId = int.Parse(fecha.ToString("yyyyMMdd"));

                var costoRow = costoTable.Rows.Find(new object[] { linea, fecha });
                var perdidasRow = perdidasTable.Rows.Find(new object[] { linea, fecha });

                if (!tramosCache.TryGetValue(linea, out var tramo))
                {
                    tramo = await _context.DimTramo.FirstOrDefaultAsync(t => t.Linea == linea && t.Linea != null);
                    if (tramo == null)
                    {
                        tramo = new DimTramo { Linea = linea };
                        _context.DimTramo.Add(tramo);
                        await _context.SaveChangesAsync();
                    }
                    tramosCache[linea] = tramo;
                }

                if (!fechasCache.TryGetValue(fechaId, out var dimFecha))
                {
                    dimFecha = await _context.DimFecha.FirstOrDefaultAsync(d => d.FechaId == fechaId && d.Fecha != null);
                    if (dimFecha == null)
                    {
                        dimFecha = new DimFecha
                        {
                            FechaId = fechaId,
                            Fecha = fecha,
                            Anio = fecha.Year,
                            Mes = fecha.Month,
                            Trimestre = (fecha.Month - 1) / 3 + 1,
                            DiaSemana = fecha.DayOfWeek.ToString()
                        };
                        _context.DimFecha.Add(dimFecha);
                    }
                    fechasCache[fechaId] = dimFecha;
                }

                var facto = new FactoConsumoCostosPerdidas
                {
                    FechaId = dimFecha.FechaId,
                    TramoId = tramo.TramoId,
                    Residencial_Consumo = GetDecimalValue(consumoRow, "Residencial_Consumo[Wh]"),
                    Comercial_Consumo = GetDecimalValue(consumoRow, "Comercial_Consumo[Wh]"),
                    Industrial_Consumo = GetDecimalValue(consumoRow, "Industrial_Consumo[Wh]"),
                    Residencial_Costo = GetDecimalValue(costoRow, "Residencial_Costo[Costo/Wh]"),
                    Comercial_Costo = GetDecimalValue(costoRow, "Comercial_Costo [Costo/Wh]"),
                    Industrial_Costo = GetDecimalValue(costoRow, "Industrial_Costo[Costo/Wh]"),
                    Residencial_Perdidas = GetDecimalValue(perdidasRow, "Residencial_Perdidas [%]"),
                    Comercial_Perdidas = GetDecimalValue(perdidasRow, "Comercial_Perdidas[%]"),
                    Industrial_Perdidas = GetDecimalValue(perdidasRow, "Industrial_Perdidas[%]")
                };

                _context.FactoConsumoCostosPerdidas.Add(facto);
            }

            await _context.SaveChangesAsync();
            return "Datos cargados exitosamente.";
        }

        private decimal GetDecimalValue(DataRow row, string columnName)
        {
            if (row == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value || string.IsNullOrWhiteSpace(row[columnName].ToString()))
            {
                return 1;
            }

            var raw = row[columnName].ToString().Trim().Replace(",", ".");

            if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                return value;

            return 0;
        }
    }
}
