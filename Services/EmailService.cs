using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using TheNextEventAPI.Data;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Models;

namespace TheNextEventAPI.Services
{
    public interface IEmailService
    {
        Task<EmailConfigurationDto> GetEmailConfigurationAsync();
        Task<EmailConfigurationDto> UpdateEmailConfigurationAsync(EmailConfigurationDto configDto);
        Task<bool> TestEmailConfigurationAsync(string testEmail);
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendFormSubmissionNotificationAsync(int formSubmissionId);
        Task<EmailStatisticsDto> GetEmailStatisticsAsync();
        Task<List<EmailHistoryDto>> GetEmailHistoryAsync(int page = 1, int pageSize = 20);
    }

    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ApplicationDbContext context, ILogger<EmailService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<EmailConfigurationDto> GetEmailConfigurationAsync()
        {
            var config = await _context.EmailConfigurations
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();

            if (config == null)
            {
                // Return default configuration
                return new EmailConfigurationDto
                {
                    SmtpServer = "smtp.gmail.com",
                    SmtpPort = 587,
                    SenderEmail = "",
                    SenderName = "The Next Event",
                    IsEnabled = false,
                    UseSSL = true
                };
            }

            return new EmailConfigurationDto
            {
                Id = config.Id,
                SmtpServer = config.SmtpServer,
                SmtpPort = config.SmtpPort,
                SenderEmail = config.SenderEmail,
                SenderName = config.SenderName,
                IsEnabled = config.IsEnabled,
                UseSSL = config.UseSSL,
                NotificationEmails = config.NotificationEmails?.Split(',').ToList() ?? new List<string>()
            };
        }

        public async Task<EmailConfigurationDto> UpdateEmailConfigurationAsync(EmailConfigurationDto configDto)
        {
            var existingConfig = await _context.EmailConfigurations
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();

            if (existingConfig != null)
            {
                existingConfig.SmtpServer = configDto.SmtpServer;
                existingConfig.SmtpPort = configDto.SmtpPort;
                existingConfig.SenderEmail = configDto.SenderEmail;
                existingConfig.SenderName = configDto.SenderName;
                existingConfig.SenderPassword = configDto.SenderPassword ?? existingConfig.SenderPassword;
                existingConfig.IsEnabled = configDto.IsEnabled;
                existingConfig.UseSSL = configDto.UseSSL;
                existingConfig.NotificationEmails = string.Join(",", configDto.NotificationEmails ?? new List<string>());
                existingConfig.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newConfig = new EmailConfiguration
                {
                    SmtpServer = configDto.SmtpServer,
                    SmtpPort = configDto.SmtpPort,
                    SenderEmail = configDto.SenderEmail,
                    SenderName = configDto.SenderName,
                    SenderPassword = configDto.SenderPassword ?? string.Empty,
                    IsEnabled = configDto.IsEnabled,
                    UseSSL = configDto.UseSSL,
                    NotificationEmails = string.Join(",", configDto.NotificationEmails ?? new List<string>()),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.EmailConfigurations.Add(newConfig);
                existingConfig = newConfig;
            }

            await _context.SaveChangesAsync();

            return new EmailConfigurationDto
            {
                Id = existingConfig.Id,
                SmtpServer = existingConfig.SmtpServer,
                SmtpPort = existingConfig.SmtpPort,
                SenderEmail = existingConfig.SenderEmail,
                SenderName = existingConfig.SenderName,
                IsEnabled = existingConfig.IsEnabled,
                UseSSL = existingConfig.UseSSL,
                NotificationEmails = existingConfig.NotificationEmails?.Split(',').ToList() ?? new List<string>()
            };
        }

        public async Task<bool> TestEmailConfigurationAsync(string testEmail)
        {
            try
            {
                var subject = "Test Email from The Next Event";
                var body = @"
                    <h2>Email Configuration Test</h2>
                    <p>This is a test email to verify your email configuration is working correctly.</p>
                    <p>If you receive this email, your SMTP settings are configured properly.</p>
                    <br>
                    <p>Best regards,<br>The Next Event Team</p>
                ";

                return await SendEmailAsync(testEmail, subject, body, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send test email to {TestEmail}", testEmail);
                return false;
            }
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var config = await GetEmailConfigurationAsync();
                
                if (!config.IsEnabled || string.IsNullOrEmpty(config.SenderEmail))
                {
                    _logger.LogWarning("Email service is disabled or not configured");
                    return false;
                }

                var fullConfig = await _context.EmailConfigurations
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefaultAsync();

                if (fullConfig == null || string.IsNullOrEmpty(fullConfig.SenderPassword))
                {
                    _logger.LogWarning("Email password not configured");
                    return false;
                }

                using var client = new SmtpClient(config.SmtpServer, config.SmtpPort);
                client.EnableSsl = config.UseSSL;
                client.Credentials = new NetworkCredential(config.SenderEmail, fullConfig.SenderPassword);

                var message = new MailMessage
                {
                    From = new MailAddress(config.SenderEmail, config.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                message.To.Add(to);

                await client.SendMailAsync(message);
                
                _logger.LogInformation("Email sent successfully to {To}", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                return false;
            }
        }

        public async Task<bool> SendFormSubmissionNotificationAsync(int formSubmissionId)
        {
            try
            {
                var submission = await _context.FormSubmissions
                    .FirstOrDefaultAsync(f => f.Id == formSubmissionId);

                if (submission == null)
                {
                    _logger.LogWarning("Form submission {FormSubmissionId} not found", formSubmissionId);
                    return false;
                }

                var config = await GetEmailConfigurationAsync();
                var notificationEmails = config.NotificationEmails;

                if (notificationEmails == null || !notificationEmails.Any())
                {
                    _logger.LogWarning("No notification emails configured");
                    return false;
                }

                var subject = $"New Form Submission - {submission.Name}";
                var body = $@"
                    <h2>New Form Submission Received</h2>
                    <table style='border-collapse: collapse; width: 100%;'>
                        <tr>
                            <td style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'><strong>Name:</strong></td>
                            <td style='border: 1px solid #ddd; padding: 8px;'>{submission.Name}</td>
                        </tr>
                        <tr>
                            <td style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'><strong>Email:</strong></td>
                            <td style='border: 1px solid #ddd; padding: 8px;'>{submission.Email}</td>
                        </tr>
                        <tr>
                            <td style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'><strong>Phone:</strong></td>
                            <td style='border: 1px solid #ddd; padding: 8px;'>{submission.Phone ?? "Not provided"}</td>
                        </tr>
                        <tr>
                            <td style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'><strong>Company:</strong></td>
                            <td style='border: 1px solid #ddd; padding: 8px;'>{submission.Company ?? "Not provided"}</td>
                        </tr>
                        <tr>
                            <td style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'><strong>Message:</strong></td>
                            <td style='border: 1px solid #ddd; padding: 8px;'>{submission.Message}</td>
                        </tr>
                        <tr>
                            <td style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'><strong>Submitted At:</strong></td>
                            <td style='border: 1px solid #ddd; padding: 8px;'>{submission.SubmittedAt:yyyy-MM-dd HH:mm:ss}</td>
                        </tr>
                    </table>
                    <br>
                    <p>Please log in to the admin dashboard to respond to this inquiry.</p>
                ";

                bool allSent = true;
                foreach (var email in notificationEmails)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        var sent = await SendEmailAsync(email.Trim(), subject, body, true);
                        if (!sent) allSent = false;
                    }
                }

                return allSent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send form submission notification for {FormSubmissionId}", formSubmissionId);
                return false;
            }
        }

        public async Task<EmailStatisticsDto> GetEmailStatisticsAsync()
        {
            // This would typically track email sends in a separate table
            // For now, return basic stats
            var totalForms = await _context.FormSubmissions.CountAsync();
            var todayForms = await _context.FormSubmissions
                .CountAsync(f => f.SubmittedAt.Date == DateTime.Today);

            return new EmailStatisticsDto
            {
                TotalEmailsSent = totalForms, // Estimate based on form submissions
                EmailsSentToday = todayForms,
                EmailsSentThisWeek = await _context.FormSubmissions
                    .CountAsync(f => f.SubmittedAt >= DateTime.Today.AddDays(-7)),
                EmailsSentThisMonth = await _context.FormSubmissions
                    .CountAsync(f => f.SubmittedAt >= DateTime.Today.AddDays(-30)),
                LastEmailSent = await _context.FormSubmissions
                    .OrderByDescending(f => f.SubmittedAt)
                    .Select(f => f.SubmittedAt)
                    .FirstOrDefaultAsync()
            };
        }

        public async Task<List<EmailHistoryDto>> GetEmailHistoryAsync(int page = 1, int pageSize = 20)
        {
            // This would typically be tracked in a separate EmailLog table
            // For now, return form submission history as email history
            var submissions = await _context.FormSubmissions
                .OrderByDescending(f => f.SubmittedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new EmailHistoryDto
                {
                    Id = f.Id,
                    To = f.Email,
                    Subject = $"Form notification for {f.Name}",
                    SentAt = f.SubmittedAt,
                    Status = "Sent",
                    Type = "Form Notification"
                })
                .ToListAsync();

            return submissions;
        }
    }
}
