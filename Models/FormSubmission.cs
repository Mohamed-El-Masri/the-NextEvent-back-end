using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheNextEventAPI.Models
{
    [Table("FormSubmissions")]
    public class FormSubmission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        [StringLength(50)]
        public string Status { get; set; } = "New";

        public bool IsRead { get; set; } = false;

        [StringLength(500)]
        public string? AdminNotes { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
