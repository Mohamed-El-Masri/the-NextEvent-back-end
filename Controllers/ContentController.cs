using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Services;

namespace TheNextEventAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        /// <summary>
        /// Get all content items
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllContent([FromQuery] string? contentKey = null, [FromQuery] bool? isActive = null)
        {
            try
            {
                var content = await _contentService.GetAllContentAsync(contentKey, isActive);
                return Ok(content);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get content by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent(int id)
        {
            try
            {
                var content = await _contentService.GetContentByIdAsync(id);
                return Ok(content);
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
        /// Get content by content key
        /// </summary>
        [HttpGet("by-key/{contentKey}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContentByKey(string contentKey)
        {
            try
            {
                var content = await _contentService.GetContentByKeyAsync(contentKey);
                return Ok(content);
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
        /// Get active content by language
        /// </summary>
        [HttpGet("by-language/{language}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContentByLanguage(string language)
        {
            try
            {
                var content = await _contentService.GetActiveContentByLanguageAsync(language);
                return Ok(content);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get content by section
        /// </summary>
        [HttpGet("section/{sectionKey}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContentBySection(string sectionKey)
        {
            try
            {
                var content = await _contentService.GetContentBySectionAsync(sectionKey);
                return Ok(content);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Create new content item
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateContent([FromBody] CreateContentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var content = await _contentService.CreateContentAsync(createDto);
                return CreatedAtAction(nameof(GetContent), new { id = content.Id }, content);
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
        /// Update existing content item
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateContent(int id, [FromBody] UpdateContentDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var content = await _contentService.UpdateContentAsync(id, updateDto);
                return Ok(content);
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
        /// Delete content item
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteContent(int id)
        {
            try
            {
                await _contentService.DeleteContentAsync(id);
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
        /// Update content sort order
        /// </summary>
        [HttpPut("{id}/sort-order")]
        [Authorize]
        public async Task<IActionResult> UpdateSortOrder(int id, [FromBody] UpdateSortOrderDto sortOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _contentService.UpdateSortOrderAsync(id, sortOrderDto.SortOrder);
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
        /// Toggle content active status
        /// </summary>
        [HttpPut("{id}/toggle-active")]
        [Authorize]
        public async Task<IActionResult> ToggleActiveStatus(int id)
        {
            try
            {
                await _contentService.ToggleActiveStatusAsync(id);
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
        /// Bulk update content items
        /// </summary>
        [HttpPut("bulk-update")]
        [Authorize]
        public async Task<IActionResult> BulkUpdateContent([FromBody] List<UpdateContentDto> updateDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _contentService.BulkUpdateContentAsync(updateDtos);
                return NoContent();
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
    }
}
