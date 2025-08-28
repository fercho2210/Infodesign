using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EnergyDataUploader.Repositories
{
    public interface IUploadRepository
    {
        Task<string> ProcessExcelUploadAsync(IFormFile file);
    }
}
