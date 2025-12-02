using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Infrastructure.Services
{
    public class ImageUrlService
    {
        private readonly string _imageBaseUrl;

        public ImageUrlService(IConfiguration configuration)
        {
            _imageBaseUrl = configuration["ImageServer:BaseUrl"] ?? "";
        }

        public string GetFullImageUrl(string imageSrc)
        {
            if (string.IsNullOrWhiteSpace(imageSrc)) return $"{_imageBaseUrl}/images/placeholder.png";
            return $"{_imageBaseUrl}/{imageSrc.TrimStart('/')}";
        }
    }

}
