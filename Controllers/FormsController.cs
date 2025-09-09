using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Services;

namespace TheNextEventAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        /// <summary>
        /// Get all form submissions with pagination and filtering
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFormSubmissions([FromQuery] FormFilterRequest filter)
        {
            try
            {
                var submissions = await _formService.GetFormSubmissionsAsync(filter);
                return Ok(submissions);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get form submission by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFormSubmission(int id)
        {
            try
            {
                var submission = await _formService.GetFormSubmissionByIdAsync(id);
                return Ok(submission);
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
        /// Submit a new form (public endpoint)
        /// </summary>
        [HttpPost("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitForm([FromBody] FormSubmissionRequest request)
        {
            try
            {
                var submission = await _formService.SubmitFormAsync(request);
                return CreatedAtAction(nameof(GetFormSubmission), new { id = submission.Id }, submission);
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
        /// Update form submission status
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateFormStatus(int id, [FromBody] UpdateFormStatusRequest request)
        {
            try
            {
                var submission = await _formService.UpdateFormStatusAsync(id, request.Status, request.AdminNotes);
                return Ok(submission);
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
        /// Mark form submission as read/unread
        /// </summary>
        [HttpPatch("{id}/read-status")]
        public async Task<IActionResult> MarkAsRead(int id, [FromBody] bool isRead)
        {
            try
            {
                var submission = await _formService.MarkAsReadAsync(id, isRead);
                return Ok(submission);
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
        /// Delete form submission
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormSubmission(int id)
        {
            try
            {
                await _formService.DeleteFormSubmissionAsync(id);
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
        /// Export form submissions to CSV
        /// </summary>
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportToCsv([FromQuery] FormFilterRequest filter)
        {
            try
            {
                var csvData = await _formService.ExportFormSubmissionsToCsvAsync(filter);
                var fileName = $"form_submissions_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                return File(csvData, "text/csv", fileName);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get form submission statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetFormStatistics()
        {
            try
            {
                var stats = await _formService.GetFormStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get daily form submission counts for dashboard charts
        /// </summary>
        [HttpGet("daily-counts")]
        public async Task<IActionResult> GetDailyFormCounts([FromQuery] int days = 30)
        {
            try
            {
                var counts = await _formService.GetDailyFormCountsAsync(days);
                return Ok(counts);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Bulk update form submissions status
        /// </summary>
        [HttpPatch("bulk-update")]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] BulkUpdateFormStatusRequest request)
        {
            try
            {
                var result = await _formService.BulkUpdateStatusAsync(request.FormIds, request.Status, request.AdminNotes);
                return Ok(new { updatedCount = result, message = $"Updated {result} form submissions" });
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
        /// Bulk delete form submissions
        /// </summary>
        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteFormRequest request)
        {
            try
            {
                var result = await _formService.BulkDeleteAsync(request.FormIds);
                return Ok(new { deletedCount = result, message = $"Deleted {result} form submissions" });
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
