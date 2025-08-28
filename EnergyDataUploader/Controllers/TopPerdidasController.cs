using EnergyDataUploader.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class TopPerdidasController : ControllerBase
{
    private readonly IReportRepository _reportRepository;

    public TopPerdidasController(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
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