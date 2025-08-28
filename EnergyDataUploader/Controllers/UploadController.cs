using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EnergyDataUploader.Repositories;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    private readonly IUploadRepository _uploadRepository;

    public UploadController(IUploadRepository uploadRepository)
    {
        _uploadRepository = uploadRepository;
    }

    [HttpPost("upload-excel")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _uploadRepository.ProcessExcelUploadAsync(file);
        return Ok(result);
    }
}
