using System;

namespace LocalThreads.Api.Utils
{
    public static class S3Helper
    {
        public static string ExtractKeyFromUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                throw new ArgumentException("Image URL cannot be null or empty");

            var uri = new Uri(imageUrl);
            return uri.AbsolutePath.TrimStart('/');
        }
    }
}
