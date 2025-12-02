namespace NetBazaar.Infrastructure.Services.Catalog
{
    public interface IImageUrlService
    {
        string Normalize(string? imageSrc);
    }

    // این سرویس فقط نرمال‌سازی انجام می‌دهد؛ اگر URL کامل ذخیره شده، همان را برمی‌گرداند.
    public class ImageUrlService : IImageUrlService
    {
        public string Normalize(string? imageSrc)
        {
            if (string.IsNullOrWhiteSpace(imageSrc))
                return "/images/placeholder.png"; // از وب‌اپ خودت، نه BaseUrl

            // اگر از ImageApi لینک کامل برگشته؛ دست‌نخورده بازگردان
            return imageSrc!;
        }
    }
}
