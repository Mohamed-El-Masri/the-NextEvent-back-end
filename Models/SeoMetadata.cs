using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheNextEventAPI.Models
{
    [Table("SeoMetadata")]
    public class SeoMetadata
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string PageUrl { get; set; } = string.Empty;

        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string TitleAR { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string DescriptionAR { get; set; } = string.Empty;

        [StringLength(500)]
        public string Keywords { get; set; } = string.Empty;

        [StringLength(500)]
        public string KeywordsAR { get; set; } = string.Empty;

        [StringLength(200)]
        public string OgTitle { get; set; } = string.Empty;

        [StringLength(200)]
        public string OgTitleAR { get; set; } = string.Empty;

        [StringLength(500)]
        public string OgDescription { get; set; } = string.Empty;

        [StringLength(500)]
        public string OgDescriptionAR { get; set; } = string.Empty;

        [StringLength(500)]
        public string OgImage { get; set; } = string.Empty;

        [StringLength(500)]
        public string CanonicalUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
