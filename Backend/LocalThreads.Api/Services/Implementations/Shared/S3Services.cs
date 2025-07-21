using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using LocalThreads.Api.Services.Interfaces.Shared;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using LocalThreads.Api.Utils;

namespace LocalThreads.Api.Services.Implementations.Shared
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration config)
        {
            _bucketName = config["AWS:BucketName"];
            _s3Client = new AmazonS3Client(
                config["AWS:AccessKey"],
                config["AWS:SecretKey"],
                Amazon.RegionEndpoint.GetBySystemName(config["AWS:Region"])
            );
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            // ✅ File validations
            var allowedTypes = new[] { "image/jpeg", "image/png", "application/pdf" };
            if (!allowedTypes.Contains(file.ContentType))
                throw new InvalidOperationException("Unsupported file type.");

            if (file.Length > 5 * 1024 * 1024)
                throw new InvalidOperationException("File size exceeds 5 MB.");

            // ✅ Safe filename generation
            var originalName = Path.GetFileNameWithoutExtension(file.FileName);
            var ext = Path.GetExtension(file.FileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var safeName = $"{originalName}_{timestamp}{ext}";

            var s3Key = $"{folderPath}/{safeName}";

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key,
                InputStream = stream,
                ContentType = file.ContentType,
            };

            await _s3Client.PutObjectAsync(request);

            return $"https://{_bucketName}.s3.amazonaws.com/{s3Key}";
        }

        public async Task DeleteFileAsync(string fileKey)
        {
            if (string.IsNullOrWhiteSpace(fileKey))
                throw new ArgumentException("File key cannot be null or empty.", nameof(fileKey));
            Console.WriteLine("Deleting file with key: " + fileKey); // "products/Men Hoodie_20250712_040617.jpg"

            var request = new DeleteObjectRequest
            {
                BucketName = "localthread-assets",
                Key = fileKey
            };

            await _s3Client.DeleteObjectAsync(request);

        }
    }
}
