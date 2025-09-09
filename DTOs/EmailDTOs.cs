using System.ComponentModel.DataAnnotations;

namespace TheNextEventAPI.DTOs
{
    public class EmailConfigurationDto
    {
        public int Id { get; set; }
        
        [Required]
        public string SmtpServer { get; set; } = string.Empty;
        
        [Required]
        public int SmtpPort { get; set; }
        
        [Required]
        [EmailAddress]
        public string SenderEmail { get; set; } = string.Empty;
        
        [Required]
        public string SenderName { get; set; } = string.Empty;
        
        public string? SenderPassword { get; set; }
        
        public bool IsEnabled { get; set; }
        public bool UseSSL { get; set; } = true;
        
        public List<string> NotificationEmails { get; set; } = new List<string>();
    }

    public class TestEmailRequest
    {
        [Required]
        [EmailAddress]
        public string TestEmail { get; set; } = string.Empty;
    }

    public class SendEmailRequest
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Body { get; set; } = string.Empty;
        
        public bool IsHtml { get; set; } = true;
    }

    public class FormSubmissionNotificationRequest
    {
        [Required]
        public int FormSubmissionId { get; set; }
    }

    public class EmailStatisticsDto
    {
        public int TotalEmailsSent { get; set; }
        public int EmailsSentToday { get; set; }
        public int EmailsSentThisWeek { get; set; }
        public int EmailsSentThisMonth { get; set; }
        public DateTime? LastEmailSent { get; set; }
    }

    public class EmailHistoryDto
    {
        public int Id { get; set; }
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
