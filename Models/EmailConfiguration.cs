using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheNextEventAPI.Models
{
    [Table("EmailConfigurations")]
    public class EmailConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SmtpServer { get; set; } = string.Empty;

        [Required]
        public int SmtpPort { get; set; }

        [Required]
        [StringLength(100)]
        public string SenderEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string SenderName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string SenderPassword { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        public bool UseSSL { get; set; } = true;

        [StringLength(1000)]
        public string? NotificationEmails { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
