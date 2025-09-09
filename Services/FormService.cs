using Microsoft.EntityFrameworkCore;
using System.Text;
using TheNextEventAPI.Data;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Models;

namespace TheNextEventAPI.Services
{
    public interface IFormService
    {
        Task<PaginatedResult<FormSubmissionDto>> GetFormSubmissionsAsync(FormFilterRequest filter);
        Task<FormSubmissionDto> GetFormSubmissionByIdAsync(int id);
        Task<FormSubmissionDto> SubmitFormAsync(FormSubmissionRequest request);
        Task<FormSubmissionDto> UpdateFormStatusAsync(int id, string status, string? adminNotes = null);
        Task<FormSubmissionDto> MarkAsReadAsync(int id, bool isRead);
        Task DeleteFormSubmissionAsync(int id);
        Task<byte[]> ExportFormSubmissionsToCsvAsync(FormFilterRequest filter);
        Task<FormStatisticsDto> GetFormStatisticsAsync();
        Task<List<DailyFormCountDto>> GetDailyFormCountsAsync(int days = 30);
        Task<int> BulkUpdateStatusAsync(List<int> formIds, string status, string? adminNotes = null);
        Task<int> BulkDeleteAsync(List<int> formIds);
    }

    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public FormService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<PaginatedResult<FormSubmissionDto>> GetFormSubmissionsAsync(FormFilterRequest filter)
        {
            var query = _context.FormSubmissions.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(f => f.Name.Contains(filter.Search) ||
                                        f.Email.Contains(filter.Search) ||
                                        f.Company!.Contains(filter.Search) ||
                                        f.Message.Contains(filter.Search));
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(f => f.Status == filter.Status);
            }

            if (filter.IsRead.HasValue)
            {
                query = query.Where(f => f.IsRead == filter.IsRead.Value);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt <= filter.EndDate.Value.AddDays(1));
            }

            var totalCount = await query.CountAsync();

            var submissions = await query
                .OrderByDescending(f => f.SubmittedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(f => new FormSubmissionDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Email = f.Email,
                    Phone = f.Phone,
                    Company = f.Company,
                    Message = f.Message,
                    Status = f.Status,
                    IsRead = f.IsRead,
                    AdminNotes = f.AdminNotes,
                    SubmittedAt = f.SubmittedAt
                })
                .ToListAsync();

            return new PaginatedResult<FormSubmissionDto>
            {
                Data = submissions,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };
        }

        public async Task<FormSubmissionDto> GetFormSubmissionByIdAsync(int id)
        {
            var submission = await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.Id == id);

            if (submission == null)
            {
                throw new ArgumentException("Form submission not found");
            }

            return new FormSubmissionDto
            {
                Id = submission.Id,
                Name = submission.Name,
                Email = submission.Email,
                Phone = submission.Phone,
                Company = submission.Company,
                Message = submission.Message,
                Status = submission.Status,
                IsRead = submission.IsRead,
                AdminNotes = submission.AdminNotes,
                SubmittedAt = submission.SubmittedAt
            };
        }

        public async Task<FormSubmissionDto> SubmitFormAsync(FormSubmissionRequest request)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required");
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new ArgumentException("Message is required");
            }

            // Create form submission
            var submission = new FormSubmission
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone?.Trim(),
                Company = request.Company?.Trim(),
                Message = request.Message.Trim(),
                Status = "New",
                IsRead = false,
                SubmittedAt = DateTime.UtcNow
            };

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            // Send notification email (fire and forget)
            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendFormSubmissionNotificationAsync(submission.Id);
                }
                catch
                {
                    // Log error but don't fail the form submission
                }
            });

            return new FormSubmissionDto
            {
                Id = submission.Id,
                Name = submission.Name,
                Email = submission.Email,
                Phone = submission.Phone,
                Company = submission.Company,
                Message = submission.Message,
                Status = submission.Status,
                IsRead = submission.IsRead,
                AdminNotes = submission.AdminNotes,
                SubmittedAt = submission.SubmittedAt
            };
        }

        public async Task<FormSubmissionDto> UpdateFormStatusAsync(int id, string status, string? adminNotes = null)
        {
            var submission = await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.Id == id);

            if (submission == null)
            {
                throw new ArgumentException("Form submission not found");
            }

            submission.Status = status;
            if (!string.IsNullOrEmpty(adminNotes))
            {
                submission.AdminNotes = adminNotes;
            }

            await _context.SaveChangesAsync();

            return new FormSubmissionDto
            {
                Id = submission.Id,
                Name = submission.Name,
                Email = submission.Email,
                Phone = submission.Phone,
                Company = submission.Company,
                Message = submission.Message,
                Status = submission.Status,
                IsRead = submission.IsRead,
                AdminNotes = submission.AdminNotes,
                SubmittedAt = submission.SubmittedAt
            };
        }

        public async Task<FormSubmissionDto> MarkAsReadAsync(int id, bool isRead)
        {
            var submission = await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.Id == id);

            if (submission == null)
            {
                throw new ArgumentException("Form submission not found");
            }

            submission.IsRead = isRead;
            await _context.SaveChangesAsync();

            return new FormSubmissionDto
            {
                Id = submission.Id,
                Name = submission.Name,
                Email = submission.Email,
                Phone = submission.Phone,
                Company = submission.Company,
                Message = submission.Message,
                Status = submission.Status,
                IsRead = submission.IsRead,
                AdminNotes = submission.AdminNotes,
                SubmittedAt = submission.SubmittedAt
            };
        }

        public async Task DeleteFormSubmissionAsync(int id)
        {
            var submission = await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.Id == id);

            if (submission == null)
            {
                throw new ArgumentException("Form submission not found");
            }

            _context.FormSubmissions.Remove(submission);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> ExportFormSubmissionsToCsvAsync(FormFilterRequest filter)
        {
            var query = _context.FormSubmissions.AsQueryable();

            // Apply same filters as GetFormSubmissionsAsync
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(f => f.Name.Contains(filter.Search) ||
                                        f.Email.Contains(filter.Search) ||
                                        f.Company!.Contains(filter.Search) ||
                                        f.Message.Contains(filter.Search));
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(f => f.Status == filter.Status);
            }

            if (filter.IsRead.HasValue)
            {
                query = query.Where(f => f.IsRead == filter.IsRead.Value);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt <= filter.EndDate.Value.AddDays(1));
            }

            var submissions = await query
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("ID,Name,Email,Phone,Company,Message,Status,Is Read,Admin Notes,Submitted At");

            foreach (var submission in submissions)
            {
                csv.AppendLine($"{submission.Id}," +
                              $"\"{submission.Name}\"," +
                              $"\"{submission.Email}\"," +
                              $"\"{submission.Phone ?? ""}\"," +
                              $"\"{submission.Company ?? ""}\"," +
                              $"\"{submission.Message.Replace("\"", "\"\"")}\"," +
                              $"\"{submission.Status}\"," +
                              $"{submission.IsRead}," +
                              $"\"{submission.AdminNotes ?? ""}\"," +
                              $"\"{submission.SubmittedAt:yyyy-MM-dd HH:mm:ss}\"");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        public async Task<FormStatisticsDto> GetFormStatisticsAsync()
        {
            var totalForms = await _context.FormSubmissions.CountAsync();
            var unreadForms = await _context.FormSubmissions.CountAsync(f => !f.IsRead);
            var todayForms = await _context.FormSubmissions
                .CountAsync(f => f.SubmittedAt.Date == DateTime.Today);
            var weekForms = await _context.FormSubmissions
                .CountAsync(f => f.SubmittedAt >= DateTime.Today.AddDays(-7));
            var monthForms = await _context.FormSubmissions
                .CountAsync(f => f.SubmittedAt >= DateTime.Today.AddDays(-30));

            var statusCounts = await _context.FormSubmissions
                .GroupBy(f => f.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);

            return new FormStatisticsDto
            {
                TotalSubmissions = totalForms,
                UnreadSubmissions = unreadForms,
                SubmissionsToday = todayForms,
                SubmissionsThisWeek = weekForms,
                SubmissionsThisMonth = monthForms,
                StatusCounts = statusCounts
            };
        }

        public async Task<List<DailyFormCountDto>> GetDailyFormCountsAsync(int days = 30)
        {
            var startDate = DateTime.Today.AddDays(-days);
            
            var counts = await _context.FormSubmissions
                .Where(f => f.SubmittedAt >= startDate)
                .GroupBy(f => f.SubmittedAt.Date)
                .Select(g => new DailyFormCountDto
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            // Fill in missing dates with zero counts
            var allDates = new List<DailyFormCountDto>();
            for (int i = 0; i < days; i++)
            {
                var date = DateTime.Today.AddDays(-i);
                var existingCount = counts.FirstOrDefault(c => c.Date == date);
                allDates.Add(new DailyFormCountDto
                {
                    Date = date,
                    Count = existingCount?.Count ?? 0
                });
            }

            return allDates.OrderBy(x => x.Date).ToList();
        }

        public async Task<int> BulkUpdateStatusAsync(List<int> formIds, string status, string? adminNotes = null)
        {
            var submissions = await _context.FormSubmissions
                .Where(f => formIds.Contains(f.Id))
                .ToListAsync();

            foreach (var submission in submissions)
            {
                submission.Status = status;
                if (!string.IsNullOrEmpty(adminNotes))
                {
                    submission.AdminNotes = adminNotes;
                }
            }

            await _context.SaveChangesAsync();
            return submissions.Count;
        }

        public async Task<int> BulkDeleteAsync(List<int> formIds)
        {
            var submissions = await _context.FormSubmissions
                .Where(f => formIds.Contains(f.Id))
                .ToListAsync();

            _context.FormSubmissions.RemoveRange(submissions);
            await _context.SaveChangesAsync();
            
            return submissions.Count;
        }
    }
}
