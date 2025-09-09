using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Services;

namespace TheNextEventAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Get current email configuration
        /// </summary>
        [HttpGet("configuration")]
        public async Task<IActionResult> GetEmailConfiguration()
        {
            try
            {
                var config = await _emailService.GetEmailConfigurationAsync();
                return Ok(config);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Update email configuration
        /// </summary>
        [HttpPut("configuration")]
        public async Task<IActionResult> UpdateEmailConfiguration([FromBody] EmailConfigurationDto configDto)
        {
            try
            {
                var config = await _emailService.UpdateEmailConfigurationAsync(configDto);
                return Ok(config);
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
        /// Test email configuration
        /// </summary>
        [HttpPost("test")]
        public async Task<IActionResult> TestEmailConfiguration([FromBody] TestEmailRequest request)
        {
            try
            {
                var result = await _emailService.TestEmailConfigurationAsync(request.TestEmail);
                return Ok(new { success = result, message = result ? "Test email sent successfully" : "Failed to send test email" });
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
        /// Send custom email
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            try
            {
                var result = await _emailService.SendEmailAsync(request.To, request.Subject, request.Body, request.IsHtml);
                return Ok(new { success = result, message = result ? "Email sent successfully" : "Failed to send email" });
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
        /// Send notification email for new form submission
        /// </summary>
        [HttpPost("notify-form-submission")]
        public async Task<IActionResult> SendFormSubmissionNotification([FromBody] FormSubmissionNotificationRequest request)
        {
            try
            {
                var result = await _emailService.SendFormSubmissionNotificationAsync(request.FormSubmissionId);
                return Ok(new { success = result, message = result ? "Notification sent successfully" : "Failed to send notification" });
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
        /// Get email sending statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetEmailStatistics()
        {
            try
            {
                var stats = await _emailService.GetEmailStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get email sending history
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetEmailHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var history = await _emailService.GetEmailHistoryAsync(page, pageSize);
                return Ok(history);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
