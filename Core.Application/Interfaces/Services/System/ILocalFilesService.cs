using Microsoft.AspNetCore.Http;

namespace Core.Application.Interfaces.Services.System
{
    public interface ILocalFilesService
    {
        Task<string> SaveAsync(IFormFile file);
        Task<string> SaveTempAsync(IFormFile file);
        Task<string> SaveAsync(string fileName, string Base64);
        bool Delete(string filePath);
    }
}
