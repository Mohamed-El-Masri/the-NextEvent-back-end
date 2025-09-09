using System.ComponentModel.DataAnnotations;

namespace TheNextEventAPI.DTOs
{
    public class FormSubmissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    public class FormSubmissionRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string? Phone { get; set; }
        public string? Company { get; set; }
        
        [Required]
        public string Message { get; set; } = string.Empty;
    }

    public class FormFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public string? Status { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateFormStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
        public string? AdminNotes { get; set; }
    }

    public class BulkUpdateFormStatusRequest
    {
        [Required]
        public List<int> FormIds { get; set; } = new List<int>();
        
        [Required]
        public string Status { get; set; } = string.Empty;
        
        public string? AdminNotes { get; set; }
    }

    public class BulkDeleteFormRequest
    {
        [Required]
        public List<int> FormIds { get; set; } = new List<int>();
    }

    public class FormStatisticsDto
    {
        public int TotalSubmissions { get; set; }
        public int UnreadSubmissions { get; set; }
        public int SubmissionsToday { get; set; }
        public int SubmissionsThisWeek { get; set; }
        public int SubmissionsThisMonth { get; set; }
        public Dictionary<string, int> StatusCounts { get; set; } = new Dictionary<string, int>();
    }

    public class DailyFormCountDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    // Additional DTOs for compatibility
    public class FormSubmissionRequestDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public string EventType { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int GuestCount { get; set; }

        [Required]
        public string Budget { get; set; } = string.Empty;

        public string EventDetails { get; set; } = string.Empty;
        public string SpecialRequirements { get; set; } = string.Empty;

        [Required]
        public string PreferredContact { get; set; } = string.Empty;
    }

    public class FormStatsDto
    {
        public int TotalSubmissions { get; set; }
        public int UnreadSubmissions { get; set; }
        public int TodaySubmissions { get; set; }
        public int ThisWeekSubmissions { get; set; }
        public int ThisMonthSubmissions { get; set; }
        public Dictionary<string, int> EventTypeBreakdown { get; set; } = new();
        public Dictionary<string, int> MonthlySubmissions { get; set; } = new();
    }
}
