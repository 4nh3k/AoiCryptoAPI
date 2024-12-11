using AoiCryptoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AoiCryptoAPI.Controllers
{
    [ApiController]
    [Route("api/v1/images")]
    public class ImageController : ControllerBase
    {
        private readonly ImageUploadService _imageUploadService;

        public ImageController(ImageUploadService imageUploadService)
        {
            _imageUploadService = imageUploadService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile image, [FromForm] string name, [FromForm] int? expiration)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image file was provided.");
            }

            try
            {
                // Convert the uploaded image to Base64
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                var imageBase64 = System.Convert.ToBase64String(memoryStream.ToArray());

                var result = await _imageUploadService.UploadImageAsync(imageBase64, name, expiration);

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Image upload failed: {ex.Message}");
            }
        }
    }
}
