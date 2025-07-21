using Microsoft.AspNetCore.Http;

namespace LocalThreads.Api.Services.Interfaces.Shared
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        Task DeleteFileAsync(string fileKey);
    }
}
