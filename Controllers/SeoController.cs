using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Services;

namespace TheNextEventAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SeoController : ControllerBase
    {
        private readonly ISeoService _seoService;

        public SeoController(ISeoService seoService)
        {
            _seoService = seoService;
        }

        /// <summary>
        /// Get all SEO metadata with pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSeoMetadata([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var metadata = await _seoService.GetSeoMetadataAsync(page, pageSize);
                return Ok(metadata);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get SEO metadata by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeoMetadata(int id)
        {
            try
            {
                var metadata = await _seoService.GetSeoMetadataByIdAsync(id);
                return Ok(metadata);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get SEO metadata by page URL
        /// </summary>
        [HttpGet("by-url")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSeoMetadataByUrl([FromQuery] string url)
        {
            try
            {
                var metadata = await _seoService.GetSeoMetadataByUrlAsync(url);
                return Ok(metadata);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Create new SEO metadata
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSeoMetadata([FromBody] SeoMetadataCreateRequest request)
        {
            try
            {
                var metadata = await _seoService.CreateSeoMetadataAsync(request);
                return CreatedAtAction(nameof(GetSeoMetadata), new { id = metadata.Id }, metadata);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Update existing SEO metadata
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeoMetadata(int id, [FromBody] SeoMetadataUpdateRequest request)
        {
            try
            {
                var metadata = await _seoService.UpdateSeoMetadataAsync(id, request);
                return Ok(metadata);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Delete SEO metadata
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeoMetadata(int id)
        {
            try
            {
                await _seoService.DeleteSeoMetadataAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Generate sitemap.xml
        /// </summary>
        [HttpGet("sitemap")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateSitemap()
        {
            try
            {
                var sitemap = await _seoService.GenerateSitemapAsync();
                return Content(sitemap, "application/xml");
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Generate robots.txt
        /// </summary>
        [HttpGet("robots")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateRobotsTxt()
        {
            try
            {
                var robotsTxt = await _seoService.GenerateRobotsTxtAsync();
                return Content(robotsTxt, "text/plain");
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Validate SEO metadata for a page
        /// </summary>
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateSeoMetadata([FromBody] SeoValidationRequest request)
        {
            try
            {
                var validation = await _seoService.ValidateSeoMetadataAsync(request);
                return Ok(validation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get SEO performance analytics
        /// </summary>
        [HttpGet("analytics")]
        public async Task<IActionResult> GetSeoAnalytics([FromQuery] int days = 30)
        {
            try
            {
                var analytics = await _seoService.GetSeoAnalyticsAsync(days);
                return Ok(analytics);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Bulk update SEO metadata
        /// </summary>
        [HttpPatch("bulk-update")]
        public async Task<IActionResult> BulkUpdateSeoMetadata([FromBody] BulkSeoUpdateRequest request)
        {
            try
            {
                var result = await _seoService.BulkUpdateSeoMetadataAsync(request);
                return Ok(new { updatedCount = result, message = $"Updated {result} SEO metadata entries" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get SEO recommendations
        /// </summary>
        [HttpGet("recommendations")]
        public async Task<IActionResult> GetSeoRecommendations()
        {
            try
            {
                var recommendations = await _seoService.GetSeoRecommendationsAsync();
                return Ok(recommendations);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
