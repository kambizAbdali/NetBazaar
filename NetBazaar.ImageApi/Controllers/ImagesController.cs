using Microsoft.AspNetCore.Mvc;

namespace NetBazaar.ImageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ImagesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // آپلود تصویر
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("فایلی انتخاب نشده است");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/uploads"); if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{Request.Scheme}://{Request.Host}/images/uploads/{fileName}";
            return Ok(new { url });
        }

        //// خواندن تصویر
        //[HttpGet("{fileName}")]
        //public IActionResult Get(string fileName)
        //{
        //    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        //    var filePath = Path.Combine(uploadsFolder, fileName);

        //    if (!System.IO.File.Exists(filePath))
        //        return NotFound();

        //    var contentType = "image/" + Path.GetExtension(fileName).Trim('.');
        //    var fileBytes = System.IO.File.ReadAllBytes(filePath);
        //    return File(fileBytes, contentType);
        //}

        // خواندن تصویر با نام فایل (اختیاری – اگر لازم شد)
        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images", "uploads", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "image/" + Path.GetExtension(fileName).Trim('.').ToLowerInvariant();
            return PhysicalFile(filePath, contentType);
        }
    }
}
