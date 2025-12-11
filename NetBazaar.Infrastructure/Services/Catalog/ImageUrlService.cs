using Microsoft.Extensions.Configuration;
using System;

namespace NetBazaar.Infrastructure.Services.Catalog
{
    public interface IImageUrlService
    {
        string Normalize(string? imageSrc, ImageType imageType);
    }

    public enum ImageType
    {
        Product,
        Logo,
        Header
    }

    // این سرویس فقط نرمال‌سازی انجام می‌دهد؛ اگر URL کامل ذخیره شده، همان را برمی‌گرداند.
    public class ImageUrlService : IImageUrlService
    {
        private readonly IConfiguration _configuration;
        private const string DefaultProductImagePath = "DefaultImages:Product";
        private const string DefaultLogoImagePath = "DefaultImages:Logo";
        private const string DefaultHeaderImagePath = "DefaultImages:Header";

        public ImageUrlService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Normalize(string? imageSrc, ImageType imageType)
        {
            // Handle null or empty input
            if (string.IsNullOrWhiteSpace(imageSrc))
            {
                return GetDefaultImagePath(imageType);
            }
            return imageSrc;

            /* Check if it's already a full URL (absolute path)
            if (IsAbsoluteUrl(imageSrc))
            {
                return imageSrc;
            }*/


            /* For relative paths, construct the full URL based on image type
            return ConstructImageUrl(imageSrc, imageType);*/
        }

        private string GetDefaultImagePath(ImageType imageType)
        {
            return imageType switch
            {
                ImageType.Product => _configuration[DefaultProductImagePath] ?? "/images/defaultProduct.png",
                ImageType.Logo => _configuration[DefaultLogoImagePath] ?? "/images/defaultlogo.png",
                ImageType.Header => _configuration[DefaultHeaderImagePath] ?? "/images/defaultheader.png",
                _ => _configuration[DefaultProductImagePath] ?? "/images/defaultproduct.png"
            };
        }

        private bool IsAbsoluteUrl(string url)
        {
            // Check if it's a full URL (http://, https://, //)
            return Uri.TryCreate(url, UriKind.Absolute, out _) ||
                   url.StartsWith("//") ||
                   url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        private string ConstructImageUrl(string relativePath, ImageType imageType)
        {
            // Remove leading slash if present for consistency
            if (relativePath.StartsWith("/"))
            {
                relativePath = relativePath.TrimStart('/');
            }

            // Get base URL from configuration based on image type
            string baseUrl = imageType switch
            {
                ImageType.Product => _configuration["ImageStorage:Products:BaseUrl"] ?? "/images/products/",
                ImageType.Logo => _configuration["ImageStorage:Logos:BaseUrl"] ?? "/images/logos/",
                ImageType.Header => _configuration["ImageStorage:Headers:BaseUrl"] ?? "/images/headers/",
                _ => _configuration["ImageStorage:Products:BaseUrl"] ?? "/images/products/"
            };

            // Ensure base URL ends with a slash
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            return $"{baseUrl}{relativePath}";
        }
    }
}