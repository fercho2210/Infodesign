using EnergyDataUploader.Models;
using Microsoft.EntityFrameworkCore;

public class ReportRepository : IReportRepository
{
    private readonly EnergyDbContext _context;

    public ReportRepository(EnergyDbContext context)
    {
        _context = context;
    }

    //Historico Consumo
    public async Task<List<FactoConsumoCostosPerdidas>> GetConsumoHistorico(DateTime fechaInicial, DateTime fechaFinal)
    {
        int fechaIdInicial = int.Parse(fechaInicial.ToString("yyyyMMdd"));
        int fechaIdFinal = int.Parse(fechaFinal.ToString("yyyyMMdd"));

        return await _context.FactoConsumoCostosPerdidas
            .Where(f => f.FechaId >= fechaIdInicial && f.FechaId <= fechaIdFinal)
            .Include(f => f.Tramo)
            .Include(f => f.Fecha)
            // === Opcional: Filtra los registros que no tienen una relación válida ===
            .Where(f => f.Tramo != null && f.Fecha != null)
            .ToListAsync();
    }

    //historico-consumo-por-cliente
    public async Task<ClienteHistoricoDto> GetConsumoPorCliente(DateTime fechaInicial, DateTime fechaFinal)
    {
        int fechaIdInicial = int.Parse(fechaInicial.ToString("yyyyMMdd"));
        int fechaIdFinal = int.Parse(fechaFinal.ToString("yyyyMMdd"));

        var datos = await _context.FactoConsumoCostosPerdidas
            .Where(f => f.FechaId >= fechaIdInicial && f.FechaId <= fechaIdFinal)
            .Include(f => f.Tramo)
            .Where(f => f.Tramo != null)
            .ToListAsync();

        var gruposPorTramo = datos.GroupBy(f => f.Tramo.Linea);

        var resultados = new ClienteHistoricoDto
        {
            Residencial = new List<TramoClienteDto>(),
            Comercial = new List<TramoClienteDto>(),
            Industrial = new List<TramoClienteDto>()
        };

        foreach (var grupo in gruposPorTramo)
        {
            var tramoDto = new TramoClienteDto
            {
                Linea = grupo.Key,
                ConsumoTotal = grupo.Sum(f => f.Residencial_Consumo),
                CostoTotal = grupo.Sum(f => f.Residencial_Costo),
                PerdidasTotal = grupo.Sum(f => f.Residencial_Perdidas)
            };
            resultados.Residencial.Add(tramoDto);

            var tramoComercialDto = new TramoClienteDto
            {
                Linea = grupo.Key,
                ConsumoTotal = grupo.Sum(f => f.Comercial_Consumo),
                CostoTotal = grupo.Sum(f => f.Comercial_Costo),
                PerdidasTotal = grupo.Sum(f => f.Comercial_Perdidas)
            };
            resultados.Comercial.Add(tramoComercialDto);

            var tramoIndustrialDto = new TramoClienteDto
            {
                Linea = grupo.Key,
                ConsumoTotal = grupo.Sum(f => f.Industrial_Consumo),
                CostoTotal = grupo.Sum(f => f.Industrial_Costo),
                PerdidasTotal = grupo.Sum(f => f.Industrial_Perdidas)
            };
            resultados.Industrial.Add(tramoIndustrialDto);
        }

        return resultados;
    }

    //Top Perdidas Por Cliente
    public async Task<TopPerdidasDto> GetTop20PerdidasPorCliente(DateTime fechaInicial, DateTime fechaFinal)
    {
        int fechaIdInicial = int.Parse(fechaInicial.ToString("yyyyMMdd"));
        int fechaIdFinal = int.Parse(fechaFinal.ToString("yyyyMMdd"));

        var datos = await _context.FactoConsumoCostosPerdidas
            .Where(f => f.FechaId >= fechaIdInicial && f.FechaId <= fechaIdFinal)
            .Include(f => f.Tramo)
            .Where(f => f.Tramo != null)
            .ToListAsync();

        var gruposPorTramo = datos.GroupBy(f => f.Tramo.Linea);

        var resultados = new TopPerdidasDto
        {
            Residencial = gruposPorTramo
                .Select(grupo => new TramoPerdidaDto
                {
                    Linea = grupo.Key,
                    PerdidasTotal = grupo.Sum(f => f.Residencial_Perdidas)
                })
                .OrderByDescending(t => t.PerdidasTotal)
               // .Take(20)
                .ToList(),

            Comercial = gruposPorTramo
                .Select(grupo => new TramoPerdidaDto
                {
                    Linea = grupo.Key,
                    PerdidasTotal = grupo.Sum(f => f.Comercial_Perdidas)
                })
                .OrderByDescending(t => t.PerdidasTotal)
               // .Take(20)
                .ToList(),

            Industrial = gruposPorTramo
                .Select(grupo => new TramoPerdidaDto
                {
                    Linea = grupo.Key,
                    PerdidasTotal = grupo.Sum(f => f.Industrial_Perdidas)
                })
                .OrderByDescending(t => t.PerdidasTotal)
               // .Take(20)
                .ToList()
        };

        return resultados;
    }

}