using System.ComponentModel.DataAnnotations;

namespace TheNextEventAPI.DTOs
{
    public class ContentDto
    {
        public int Id { get; set; }
        public string ContentKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameAR { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionAR { get; set; } = string.Empty;
        public string MediaUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ContentItemDto
    {
        public int Id { get; set; }
        public string SectionKey { get; set; } = string.Empty;
        public string ContentKey { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? NameAR { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAR { get; set; }
        public string? MediaUrl { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public string LastUpdated { get; set; } = string.Empty;
    }

    public class ContentSectionDto
    {
        public string SectionKey { get; set; } = string.Empty;
        public List<ContentItemDto> Items { get; set; } = new List<ContentItemDto>();
    }

    public class ContentUpdateRequest
    {
        public string? ContentKey { get; set; }
        public string? Name { get; set; }
        public string? NameAR { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAR { get; set; }
        public string? MediaUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
    }

    public class CreateContentDto
    {
        [Required]
        public string SectionKey { get; set; } = string.Empty;
        
        [Required]
        public string ContentKey { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string NameAR { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public string DescriptionAR { get; set; } = string.Empty;
        
        public string MediaUrl { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
    }

    public class UpdateContentDto
    {
        public int? Id { get; set; }
        
        public string? SectionKey { get; set; }
        
        public string? ContentKey { get; set; }
        
        public string? Name { get; set; }
        
        public string? NameAR { get; set; }
        
        public string? Description { get; set; }
        
        public string? DescriptionAR { get; set; }
        
        public string? MediaUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
    }
}
