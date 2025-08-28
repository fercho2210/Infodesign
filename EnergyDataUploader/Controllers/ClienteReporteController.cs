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
}