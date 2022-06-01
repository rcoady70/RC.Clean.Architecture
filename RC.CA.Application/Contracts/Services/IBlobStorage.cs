using Microsoft.AspNetCore.Http;

namespace RC.CA.Application.Contracts.Services
{
    public interface IBlobStorage
    {
        Task<string> GetFileHeaderAsync(string containerName, string fileName);
        Task UploadAsync(string containerName, string fileName, IFormFile? uploadedFile);
    }
}
