using EnergyDataUploader.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ClienteReporteController : ControllerBase
{
    private readonly IReportRepository _reportRepository;

    public ClienteReporteController(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    [HttpGet("historico-consumo-por-cliente")]
    public async Task<IActionResult> GetHistoricoConsumoPorCliente(string fechaInicial, string fechaFinal)
    {
        if (!DateTime.TryParse(fechaInicial, out DateTime start) || !DateTime.TryParse(fechaFinal, out DateTime end))
        {
            return BadRequest("El formato de fecha no es válido. Utiliza yyyy-MM-dd.");
        }

        var resultados = await _reportRepository.GetConsumoPorCliente(start, end);

        return Ok(resultados);
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

    [HttpGet("top-peores-tramos")]
    public async Task<IActionResult> GetTop20Perdidas(string fechaInicial, string fechaFinal)
    {
        if (!DateTime.TryParse(fechaInicial, out DateTime start) || !DateTime.TryParse(fechaFinal, out DateTime end))
        {
            return BadRequest("El formato de fecha no es válido. Utiliza yyyy-MM-dd.");
        }

        var resultados = await _reportRepository.GetTop20PerdidasPorCliente(start, end);

        return Ok(resultados);
    }
}