using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Services;

namespace TheNextEventAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public MediaController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [Authorize]
        [HttpGet("signature")]
        public IActionResult GetSignature([FromQuery] string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                return BadRequest(new { message = "PublicId is required" });
            }

            var signature = _cloudinaryService.GenerateSignature(publicId);
            return Ok(signature);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<List<MediaItemDto>>> GetAllMedia()
        {
            var media = await _cloudinaryService.GetAllImagesAsync();
            return Ok(media);
        }

        [Authorize]
        [HttpDelete("{publicId}")]
        public async Task<IActionResult> DeleteMedia(string publicId)
        {
            var success = await _cloudinaryService.DeleteImageAsync(publicId);
            
            if (!success)
            {
                return BadRequest(new { message = "Failed to delete image" });
            }

            return Ok(new { message = "Image deleted successfully" });
        }
    }
}
