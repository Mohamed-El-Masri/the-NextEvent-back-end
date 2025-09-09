using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml;
using TheNextEventAPI.Data;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Models;

namespace TheNextEventAPI.Services
{
    public interface ISeoService
    {
        Task<PaginatedResult<SeoMetadataDto>> GetSeoMetadataAsync(int page = 1, int pageSize = 20);
        Task<SeoMetadataDto> GetSeoMetadataByIdAsync(int id);
        Task<SeoMetadataDto> GetSeoMetadataByUrlAsync(string url);
        Task<SeoMetadataDto> CreateSeoMetadataAsync(SeoMetadataCreateRequest request);
        Task<SeoMetadataDto> UpdateSeoMetadataAsync(int id, SeoMetadataUpdateRequest request);
        Task DeleteSeoMetadataAsync(int id);
        Task<string> GenerateSitemapAsync();
        Task<string> GenerateRobotsTxtAsync();
        Task<SeoValidationResultDto> ValidateSeoMetadataAsync(SeoValidationRequest request);
        Task<SeoAnalyticsDto> GetSeoAnalyticsAsync(int days = 30);
        Task<int> BulkUpdateSeoMetadataAsync(BulkSeoUpdateRequest request);
        Task<List<SeoRecommendationDto>> GetSeoRecommendationsAsync();
    }

    public class SeoService : ISeoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SeoService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<PaginatedResult<SeoMetadataDto>> GetSeoMetadataAsync(int page = 1, int pageSize = 20)
        {
            var query = _context.SeoMetadata.AsQueryable();
            var totalCount = await query.CountAsync();

            var seoData = await query
                .OrderBy(s => s.PageUrl)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SeoMetadataDto
                {
                    Id = s.Id,
                    PageUrl = s.PageUrl,
                    Title = s.Title,
                    TitleAR = s.TitleAR,
                    Description = s.Description,
                    DescriptionAR = s.DescriptionAR,
                    Keywords = s.Keywords,
                    KeywordsAR = s.KeywordsAR,
                    OgTitle = s.OgTitle,
                    OgTitleAR = s.OgTitleAR,
                    OgDescription = s.OgDescription,
                    OgDescriptionAR = s.OgDescriptionAR,
                    OgImage = s.OgImage,
                    CanonicalUrl = s.CanonicalUrl,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            return new PaginatedResult<SeoMetadataDto>
            {
                Data = seoData,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<SeoMetadataDto> GetSeoMetadataByIdAsync(int id)
        {
            var seoData = await _context.SeoMetadata
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seoData == null)
            {
                throw new ArgumentException("SEO metadata not found");
            }

            return new SeoMetadataDto
            {
                Id = seoData.Id,
                PageUrl = seoData.PageUrl,
                Title = seoData.Title,
                TitleAR = seoData.TitleAR,
                Description = seoData.Description,
                DescriptionAR = seoData.DescriptionAR,
                Keywords = seoData.Keywords,
                KeywordsAR = seoData.KeywordsAR,
                OgTitle = seoData.OgTitle,
                OgTitleAR = seoData.OgTitleAR,
                OgDescription = seoData.OgDescription,
                OgDescriptionAR = seoData.OgDescriptionAR,
                OgImage = seoData.OgImage,
                CanonicalUrl = seoData.CanonicalUrl,
                IsActive = seoData.IsActive,
                CreatedAt = seoData.CreatedAt,
                UpdatedAt = seoData.UpdatedAt
            };
        }

        public async Task<SeoMetadataDto> GetSeoMetadataByUrlAsync(string url)
        {
            var seoData = await _context.SeoMetadata
                .FirstOrDefaultAsync(s => s.PageUrl == url && s.IsActive);

            if (seoData == null)
            {
                throw new ArgumentException("SEO metadata not found for this URL");
            }

            return new SeoMetadataDto
            {
                Id = seoData.Id,
                PageUrl = seoData.PageUrl,
                Title = seoData.Title,
                TitleAR = seoData.TitleAR,
                Description = seoData.Description,
                DescriptionAR = seoData.DescriptionAR,
                Keywords = seoData.Keywords,
                KeywordsAR = seoData.KeywordsAR,
                OgTitle = seoData.OgTitle,
                OgTitleAR = seoData.OgTitleAR,
                OgDescription = seoData.OgDescription,
                OgDescriptionAR = seoData.OgDescriptionAR,
                OgImage = seoData.OgImage,
                CanonicalUrl = seoData.CanonicalUrl,
                IsActive = seoData.IsActive,
                CreatedAt = seoData.CreatedAt,
                UpdatedAt = seoData.UpdatedAt
            };
        }

        public async Task<SeoMetadataDto> CreateSeoMetadataAsync(SeoMetadataCreateRequest request)
        {
            // Check if URL already exists
            var existingSeo = await _context.SeoMetadata
                .FirstOrDefaultAsync(s => s.PageUrl == request.PageUrl);

            if (existingSeo != null)
            {
                throw new ArgumentException("SEO metadata for this URL already exists");
            }

            var seoData = new SeoMetadata
            {
                PageUrl = request.PageUrl,
                Title = request.Title ?? string.Empty,
                TitleAR = request.TitleAR ?? string.Empty,
                Description = request.Description ?? string.Empty,
                DescriptionAR = request.DescriptionAR ?? string.Empty,
                Keywords = request.Keywords ?? string.Empty,
                KeywordsAR = request.KeywordsAR ?? string.Empty,
                OgTitle = request.OgTitle ?? string.Empty,
                OgTitleAR = request.OgTitleAR ?? string.Empty,
                OgDescription = request.OgDescription ?? string.Empty,
                OgDescriptionAR = request.OgDescriptionAR ?? string.Empty,
                OgImage = request.OgImage ?? string.Empty,
                CanonicalUrl = request.CanonicalUrl ?? string.Empty,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SeoMetadata.Add(seoData);
            await _context.SaveChangesAsync();

            return new SeoMetadataDto
            {
                Id = seoData.Id,
                PageUrl = seoData.PageUrl,
                Title = seoData.Title,
                TitleAR = seoData.TitleAR,
                Description = seoData.Description,
                DescriptionAR = seoData.DescriptionAR,
                Keywords = seoData.Keywords,
                KeywordsAR = seoData.KeywordsAR,
                OgTitle = seoData.OgTitle,
                OgTitleAR = seoData.OgTitleAR,
                OgDescription = seoData.OgDescription,
                OgDescriptionAR = seoData.OgDescriptionAR,
                OgImage = seoData.OgImage,
                CanonicalUrl = seoData.CanonicalUrl,
                IsActive = seoData.IsActive,
                CreatedAt = seoData.CreatedAt,
                UpdatedAt = seoData.UpdatedAt
            };
        }

        public async Task<SeoMetadataDto> UpdateSeoMetadataAsync(int id, SeoMetadataUpdateRequest request)
        {
            var seoData = await _context.SeoMetadata
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seoData == null)
            {
                throw new ArgumentException("SEO metadata not found");
            }

            // Check if URL is being changed and if it already exists
            if (request.PageUrl != seoData.PageUrl)
            {
                var existingSeo = await _context.SeoMetadata
                    .FirstOrDefaultAsync(s => s.PageUrl == request.PageUrl && s.Id != id);

                if (existingSeo != null)
                {
                    throw new ArgumentException("SEO metadata for this URL already exists");
                }
            }

            seoData.PageUrl = request.PageUrl;
            seoData.Title = request.Title ?? seoData.Title;
            seoData.TitleAR = request.TitleAR ?? seoData.TitleAR;
            seoData.Description = request.Description ?? seoData.Description;
            seoData.DescriptionAR = request.DescriptionAR ?? seoData.DescriptionAR;
            seoData.Keywords = request.Keywords ?? seoData.Keywords;
            seoData.KeywordsAR = request.KeywordsAR ?? seoData.KeywordsAR;
            seoData.OgTitle = request.OgTitle ?? seoData.OgTitle;
            seoData.OgTitleAR = request.OgTitleAR ?? seoData.OgTitleAR;
            seoData.OgDescription = request.OgDescription ?? seoData.OgDescription;
            seoData.OgDescriptionAR = request.OgDescriptionAR ?? seoData.OgDescriptionAR;
            seoData.OgImage = request.OgImage ?? seoData.OgImage;
            seoData.CanonicalUrl = request.CanonicalUrl ?? seoData.CanonicalUrl;
            seoData.IsActive = request.IsActive;
            seoData.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new SeoMetadataDto
            {
                Id = seoData.Id,
                PageUrl = seoData.PageUrl,
                Title = seoData.Title,
                TitleAR = seoData.TitleAR,
                Description = seoData.Description,
                DescriptionAR = seoData.DescriptionAR,
                Keywords = seoData.Keywords,
                KeywordsAR = seoData.KeywordsAR,
                OgTitle = seoData.OgTitle,
                OgTitleAR = seoData.OgTitleAR,
                OgDescription = seoData.OgDescription,
                OgDescriptionAR = seoData.OgDescriptionAR,
                OgImage = seoData.OgImage,
                CanonicalUrl = seoData.CanonicalUrl,
                IsActive = seoData.IsActive,
                CreatedAt = seoData.CreatedAt,
                UpdatedAt = seoData.UpdatedAt
            };
        }

        public async Task DeleteSeoMetadataAsync(int id)
        {
            var seoData = await _context.SeoMetadata
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seoData == null)
            {
                throw new ArgumentException("SEO metadata not found");
            }

            _context.SeoMetadata.Remove(seoData);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateSitemapAsync()
        {
            var baseUrl = _configuration["BaseUrl"] ?? "https://thenextevent.com";
            var seoPages = await _context.SeoMetadata
                .Where(s => s.IsActive)
                .ToListAsync();

            var sitemap = new StringBuilder();
            sitemap.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sitemap.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // Add homepage
            sitemap.AppendLine("  <url>");
            sitemap.AppendLine($"    <loc>{baseUrl}</loc>");
            sitemap.AppendLine($"    <lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>");
            sitemap.AppendLine("    <changefreq>weekly</changefreq>");
            sitemap.AppendLine("    <priority>1.0</priority>");
            sitemap.AppendLine("  </url>");

            // Add other pages
            foreach (var page in seoPages)
            {
                if (page.PageUrl != "/" && page.PageUrl != "")
                {
                    sitemap.AppendLine("  <url>");
                    sitemap.AppendLine($"    <loc>{baseUrl}{page.PageUrl}</loc>");
                    sitemap.AppendLine($"    <lastmod>{page.UpdatedAt:yyyy-MM-dd}</lastmod>");
                    sitemap.AppendLine("    <changefreq>monthly</changefreq>");
                    sitemap.AppendLine("    <priority>0.8</priority>");
                    sitemap.AppendLine("  </url>");
                }
            }

            sitemap.AppendLine("</urlset>");
            return sitemap.ToString();
        }

        public async Task<string> GenerateRobotsTxtAsync()
        {
            var baseUrl = _configuration["BaseUrl"] ?? "https://thenextevent.com";
            
            var robotsTxt = new StringBuilder();
            robotsTxt.AppendLine("User-agent: *");
            robotsTxt.AppendLine("Allow: /");
            robotsTxt.AppendLine("");
            robotsTxt.AppendLine($"Sitemap: {baseUrl}/api/seo/sitemap");
            
            return robotsTxt.ToString();
        }

        public async Task<SeoValidationResultDto> ValidateSeoMetadataAsync(SeoValidationRequest request)
        {
            var validation = new SeoValidationResultDto
            {
                IsValid = true,
                Issues = new List<string>(),
                Recommendations = new List<string>()
            };

            // Title validation
            if (string.IsNullOrEmpty(request.Title))
            {
                validation.IsValid = false;
                validation.Issues.Add("Title is required");
            }
            else if (request.Title.Length < 30)
            {
                validation.Recommendations.Add("Title should be at least 30 characters for better SEO");
            }
            else if (request.Title.Length > 60)
            {
                validation.Recommendations.Add("Title should be under 60 characters to avoid truncation");
            }

            // Description validation
            if (string.IsNullOrEmpty(request.Description))
            {
                validation.IsValid = false;
                validation.Issues.Add("Meta description is required");
            }
            else if (request.Description.Length < 120)
            {
                validation.Recommendations.Add("Meta description should be at least 120 characters");
            }
            else if (request.Description.Length > 160)
            {
                validation.Recommendations.Add("Meta description should be under 160 characters");
            }

            // Keywords validation
            if (string.IsNullOrEmpty(request.Keywords))
            {
                validation.Recommendations.Add("Consider adding relevant keywords");
            }

            // OG validation
            if (string.IsNullOrEmpty(request.OgTitle))
            {
                validation.Recommendations.Add("Open Graph title helps with social media sharing");
            }

            if (string.IsNullOrEmpty(request.OgDescription))
            {
                validation.Recommendations.Add("Open Graph description helps with social media sharing");
            }

            if (string.IsNullOrEmpty(request.OgImage))
            {
                validation.Recommendations.Add("Open Graph image is important for social media sharing");
            }

            return validation;
        }

        public async Task<SeoAnalyticsDto> GetSeoAnalyticsAsync(int days = 30)
        {
            var totalPages = await _context.SeoMetadata.CountAsync();
            var activePages = await _context.SeoMetadata.CountAsync(s => s.IsActive);
            var recentlyUpdated = await _context.SeoMetadata
                .CountAsync(s => s.UpdatedAt >= DateTime.UtcNow.AddDays(-days));

            return new SeoAnalyticsDto
            {
                TotalPages = totalPages,
                ActivePages = activePages,
                InactivePages = totalPages - activePages,
                RecentlyUpdatedPages = recentlyUpdated,
                PagesWithoutTitle = await _context.SeoMetadata.CountAsync(s => string.IsNullOrEmpty(s.Title)),
                PagesWithoutDescription = await _context.SeoMetadata.CountAsync(s => string.IsNullOrEmpty(s.Description)),
                PagesWithoutKeywords = await _context.SeoMetadata.CountAsync(s => string.IsNullOrEmpty(s.Keywords)),
                PagesWithoutOgImage = await _context.SeoMetadata.CountAsync(s => string.IsNullOrEmpty(s.OgImage))
            };
        }

        public async Task<int> BulkUpdateSeoMetadataAsync(BulkSeoUpdateRequest request)
        {
            var seoPages = await _context.SeoMetadata
                .Where(s => request.PageIds.Contains(s.Id))
                .ToListAsync();

            foreach (var page in seoPages)
            {
                if (!string.IsNullOrEmpty(request.Title))
                    page.Title = request.Title;
                if (!string.IsNullOrEmpty(request.TitleAR))
                    page.TitleAR = request.TitleAR;
                if (!string.IsNullOrEmpty(request.Description))
                    page.Description = request.Description;
                if (!string.IsNullOrEmpty(request.DescriptionAR))
                    page.DescriptionAR = request.DescriptionAR;
                if (!string.IsNullOrEmpty(request.Keywords))
                    page.Keywords = request.Keywords;
                if (!string.IsNullOrEmpty(request.KeywordsAR))
                    page.KeywordsAR = request.KeywordsAR;
                if (request.IsActive.HasValue)
                    page.IsActive = request.IsActive.Value;
                
                page.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return seoPages.Count;
        }

        public async Task<List<SeoRecommendationDto>> GetSeoRecommendationsAsync()
        {
            var recommendations = new List<SeoRecommendationDto>();

            // Check for pages without titles
            var pagesWithoutTitle = await _context.SeoMetadata
                .Where(s => string.IsNullOrEmpty(s.Title))
                .CountAsync();

            if (pagesWithoutTitle > 0)
            {
                recommendations.Add(new SeoRecommendationDto
                {
                    Type = "Missing Title",
                    Message = $"{pagesWithoutTitle} pages are missing titles",
                    Priority = "High",
                    Action = "Add descriptive titles to improve SEO"
                });
            }

            // Check for pages without descriptions
            var pagesWithoutDescription = await _context.SeoMetadata
                .Where(s => string.IsNullOrEmpty(s.Description))
                .CountAsync();

            if (pagesWithoutDescription > 0)
            {
                recommendations.Add(new SeoRecommendationDto
                {
                    Type = "Missing Description",
                    Message = $"{pagesWithoutDescription} pages are missing meta descriptions",
                    Priority = "High",
                    Action = "Add meta descriptions (120-160 characters)"
                });
            }

            // Check for pages without OG images
            var pagesWithoutOgImage = await _context.SeoMetadata
                .Where(s => string.IsNullOrEmpty(s.OgImage))
                .CountAsync();

            if (pagesWithoutOgImage > 0)
            {
                recommendations.Add(new SeoRecommendationDto
                {
                    Type = "Missing OG Image",
                    Message = $"{pagesWithoutOgImage} pages are missing Open Graph images",
                    Priority = "Medium",
                    Action = "Add images for better social media sharing"
                });
            }

            // Check for short titles
            var shortTitles = await _context.SeoMetadata
                .Where(s => s.Title.Length < 30 && s.Title.Length > 0)
                .CountAsync();

            if (shortTitles > 0)
            {
                recommendations.Add(new SeoRecommendationDto
                {
                    Type = "Short Titles",
                    Message = $"{shortTitles} pages have titles shorter than 30 characters",
                    Priority = "Medium",
                    Action = "Expand titles to 30-60 characters for better SEO"
                });
            }

            return recommendations;
        }
    }
}
