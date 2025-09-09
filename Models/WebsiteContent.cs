using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheNextEventAPI.Models
{
    [Table("WebsiteContents")]
    public class WebsiteContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ContentKey { get; set; } = string.Empty;

        [StringLength(100)]
        public string SectionKey { get; set; } = string.Empty;

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string NameAR { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000)]
        public string DescriptionAR { get; set; } = string.Empty;

        [StringLength(500)]
        public string MediaUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
