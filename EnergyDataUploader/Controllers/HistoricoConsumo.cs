// Controllers/ReporteController.cs
using EnergyDataUploader.Models;
using EnergyDataUploader.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ReporteController : ControllerBase
{
    private readonly IReportRepository _reportRepository;

    public ReporteController(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    [HttpGet("historico-consumo")]
    public async Task<IActionResult> GetHistoricoConsumo(string fechaInicial, string fechaFinal)
    {
        // Valida y convierte las fechas
        if (!DateTime.TryParse(fechaInicial, out DateTime start) || !DateTime.TryParse(fechaFinal, out DateTime end))
        {
            return BadRequest("El formato de fecha no es válido. Utiliza yyyy-MM-dd.");
        }

        // Obtiene los datos del repositorio
        var historico = await _reportRepository.GetConsumoHistorico(start, end);

        // === CORRECCIÓN CLAVE: Agrega ?. para evitar la excepción de nulo ===
        var resultados = historico.Select(f => new TramoHistoricoDto
        {
            Linea = f.Tramo?.Linea,
            Fecha = f.Fecha?.Fecha ?? default, // Usa un valor por defecto si es nulo
            ConsumoResidencial = f.Residencial_Consumo,
            ConsumoComercial = f.Comercial_Consumo,
            ConsumoIndustrial = f.Industrial_Consumo,
            CostoResidencial = f.Residencial_Costo,
            CostoComercial = f.Comercial_Costo,
            CostoIndustrial = f.Industrial_Costo,
            PerdidasResidenciales = f.Residencial_Perdidas,
            PerdidasComerciales = f.Comercial_Perdidas,
            PerdidasIndustriales = f.Industrial_Perdidas
        }).ToList();

        return Ok(resultados);
    }
}