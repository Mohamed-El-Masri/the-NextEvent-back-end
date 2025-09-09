using System.ComponentModel.DataAnnotations;

namespace TheNextEventAPI.DTOs
{
    public class SeoMetadataDto
    {
        public int Id { get; set; }
        public string PageUrl { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleAR { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionAR { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string KeywordsAR { get; set; } = string.Empty;
        public string OgTitle { get; set; } = string.Empty;
        public string OgTitleAR { get; set; } = string.Empty;
        public string OgDescription { get; set; } = string.Empty;
        public string OgDescriptionAR { get; set; } = string.Empty;
        public string OgImage { get; set; } = string.Empty;
        public string CanonicalUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SeoMetadataCreateRequest
    {
        [Required]
        public string PageUrl { get; set; } = string.Empty;
        
        public string? Title { get; set; }
        public string? TitleAR { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAR { get; set; }
        public string? Keywords { get; set; }
        public string? KeywordsAR { get; set; }
        public string? OgTitle { get; set; }
        public string? OgTitleAR { get; set; }
        public string? OgDescription { get; set; }
        public string? OgDescriptionAR { get; set; }
        public string? OgImage { get; set; }
        public string? CanonicalUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class SeoMetadataUpdateRequest
    {
        [Required]
        public string PageUrl { get; set; } = string.Empty;
        
        public string? Title { get; set; }
        public string? TitleAR { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAR { get; set; }
        public string? Keywords { get; set; }
        public string? KeywordsAR { get; set; }
        public string? OgTitle { get; set; }
        public string? OgTitleAR { get; set; }
        public string? OgDescription { get; set; }
        public string? OgDescriptionAR { get; set; }
        public string? OgImage { get; set; }
        public string? CanonicalUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class SeoValidationRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string OgTitle { get; set; } = string.Empty;
        public string OgDescription { get; set; } = string.Empty;
        public string OgImage { get; set; } = string.Empty;
    }

    public class SeoValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Issues { get; set; } = new List<string>();
        public List<string> Recommendations { get; set; } = new List<string>();
    }

    public class SeoAnalyticsDto
    {
        public int TotalPages { get; set; }
        public int ActivePages { get; set; }
        public int InactivePages { get; set; }
        public int RecentlyUpdatedPages { get; set; }
        public int PagesWithoutTitle { get; set; }
        public int PagesWithoutDescription { get; set; }
        public int PagesWithoutKeywords { get; set; }
        public int PagesWithoutOgImage { get; set; }
    }

    public class BulkSeoUpdateRequest
    {
        [Required]
        public List<int> PageIds { get; set; } = new List<int>();
        
        public string? Title { get; set; }
        public string? TitleAR { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAR { get; set; }
        public string? Keywords { get; set; }
        public string? KeywordsAR { get; set; }
        public bool? IsActive { get; set; }
    }

    public class SeoRecommendationDto
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
