using Microsoft.EntityFrameworkCore;
using TheNextEventAPI.Data;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Models;

namespace TheNextEventAPI.Services
{
    public interface IContentService
    {
        Task<List<ContentDto>> GetAllContentAsync(string? contentKey = null, bool? isActive = null);
        Task<ContentDto> GetContentByIdAsync(int id);
        Task<ContentDto> GetContentByKeyAsync(string contentKey);
        Task<List<ContentDto>> GetActiveContentByLanguageAsync(string language);
        Task<List<ContentDto>> GetContentBySectionAsync(string sectionKey);
        Task<ContentDto> CreateContentAsync(CreateContentDto createDto);
        Task<ContentDto> UpdateContentAsync(int id, UpdateContentDto updateDto);
        Task DeleteContentAsync(int id);
        Task UpdateSortOrderAsync(int id, int sortOrder);
        Task ToggleActiveStatusAsync(int id);
        Task BulkUpdateContentAsync(List<UpdateContentDto> updateDtos);
        
        // Legacy methods for backward compatibility
        Task<ContentDto> CreateContentAsync(ContentUpdateRequest request);
        Task<ContentDto> UpdateContentAsync(int id, ContentUpdateRequest request);
        Task<ContentDto> ToggleContentStatusAsync(int id, bool isActive);
        Task ReorderContentAsync(List<int> orderedIds);
    }

    public class ContentService : IContentService
    {
        private readonly ApplicationDbContext _context;

        public ContentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContentDto>> GetAllContentAsync(string? contentKey = null, bool? isActive = null)
        {
            var query = _context.WebsiteContents.AsQueryable();

            if (!string.IsNullOrEmpty(contentKey))
            {
                query = query.Where(c => c.ContentKey.Contains(contentKey));
            }

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            var contents = await query
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.ContentKey)
                .Select(c => new ContentDto
                {
                    Id = c.Id,
                    ContentKey = c.ContentKey,
                    Name = c.Name,
                    NameAR = c.NameAR,
                    Description = c.Description,
                    DescriptionAR = c.DescriptionAR,
                    MediaUrl = c.MediaUrl,
                    IsActive = c.IsActive,
                    SortOrder = c.SortOrder,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();

            return contents;
        }

        public async Task<ContentDto> GetContentByIdAsync(int id)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            return new ContentDto
            {
                Id = content.Id,
                ContentKey = content.ContentKey,
                Name = content.Name,
                NameAR = content.NameAR,
                Description = content.Description,
                DescriptionAR = content.DescriptionAR,
                MediaUrl = content.MediaUrl,
                IsActive = content.IsActive,
                SortOrder = content.SortOrder,
                CreatedAt = content.CreatedAt,
                UpdatedAt = content.UpdatedAt
            };
        }

        public async Task<ContentDto> GetContentByKeyAsync(string contentKey)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.ContentKey == contentKey && c.IsActive);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            return new ContentDto
            {
                Id = content.Id,
                ContentKey = content.ContentKey,
                Name = content.Name,
                NameAR = content.NameAR,
                Description = content.Description,
                DescriptionAR = content.DescriptionAR,
                MediaUrl = content.MediaUrl,
                IsActive = content.IsActive,
                SortOrder = content.SortOrder,
                CreatedAt = content.CreatedAt,
                UpdatedAt = content.UpdatedAt
            };
        }

        public async Task<ContentDto> CreateContentAsync(ContentUpdateRequest request)
        {
            // Validate required fields
            if (string.IsNullOrEmpty(request.ContentKey))
            {
                throw new ArgumentException("Content key is required");
            }

            // Check if content key already exists
            var existingContent = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.ContentKey == request.ContentKey);

            if (existingContent != null)
            {
                throw new ArgumentException("Content with this key already exists");
            }

            var content = new WebsiteContent
            {
                ContentKey = request.ContentKey,
                Name = request.Name ?? string.Empty,
                NameAR = request.NameAR ?? string.Empty,
                Description = request.Description ?? string.Empty,
                DescriptionAR = request.DescriptionAR ?? string.Empty,
                MediaUrl = request.MediaUrl ?? string.Empty,
                IsActive = request.IsActive,
                SortOrder = request.SortOrder,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.WebsiteContents.Add(content);
            await _context.SaveChangesAsync();

            return new ContentDto
            {
                Id = content.Id,
                ContentKey = content.ContentKey,
                Name = content.Name,
                NameAR = content.NameAR,
                Description = content.Description,
                DescriptionAR = content.DescriptionAR,
                MediaUrl = content.MediaUrl,
                IsActive = content.IsActive,
                SortOrder = content.SortOrder,
                CreatedAt = content.CreatedAt,
                UpdatedAt = content.UpdatedAt
            };
        }

        public async Task<ContentDto> UpdateContentAsync(int id, ContentUpdateRequest request)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            // Check if content key is being changed and if it already exists
            if (!string.IsNullOrEmpty(request.ContentKey) && request.ContentKey != content.ContentKey)
            {
                var existingContent = await _context.WebsiteContents
                    .FirstOrDefaultAsync(c => c.ContentKey == request.ContentKey && c.Id != id);

                if (existingContent != null)
                {
                    throw new ArgumentException("Content with this key already exists");
                }
                content.ContentKey = request.ContentKey;
            }

            content.Name = request.Name ?? content.Name;
            content.NameAR = request.NameAR ?? content.NameAR;
            content.Description = request.Description ?? content.Description;
            content.DescriptionAR = request.DescriptionAR ?? content.DescriptionAR;
            content.MediaUrl = request.MediaUrl ?? content.MediaUrl;
            content.IsActive = request.IsActive;
            content.SortOrder = request.SortOrder;
            content.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ContentDto
            {
                Id = content.Id,
                ContentKey = content.ContentKey,
                Name = content.Name,
                NameAR = content.NameAR,
                Description = content.Description,
                DescriptionAR = content.DescriptionAR,
                MediaUrl = content.MediaUrl,
                IsActive = content.IsActive,
                SortOrder = content.SortOrder,
                CreatedAt = content.CreatedAt,
                UpdatedAt = content.UpdatedAt
            };
        }

        public async Task DeleteContentAsync(int id)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            _context.WebsiteContents.Remove(content);
            await _context.SaveChangesAsync();
        }

        public async Task<ContentDto> ToggleContentStatusAsync(int id, bool isActive)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            content.IsActive = isActive;
            content.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ContentDto
            {
                Id = content.Id,
                ContentKey = content.ContentKey,
                Name = content.Name,
                NameAR = content.NameAR,
                Description = content.Description,
                DescriptionAR = content.DescriptionAR,
                MediaUrl = content.MediaUrl,
                IsActive = content.IsActive,
                SortOrder = content.SortOrder,
                CreatedAt = content.CreatedAt,
                UpdatedAt = content.UpdatedAt
            };
        }

        public async Task ReorderContentAsync(List<int> orderedIds)
        {
            var contents = await _context.WebsiteContents
                .Where(c => orderedIds.Contains(c.Id))
                .ToListAsync();

            for (int i = 0; i < orderedIds.Count; i++)
            {
                var content = contents.FirstOrDefault(c => c.Id == orderedIds[i]);
                if (content != null)
                {
                    content.SortOrder = i + 1;
                    content.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ContentDto>> GetActiveContentByLanguageAsync(string language)
        {
            var contents = await _context.WebsiteContents
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.ContentKey)
                .ToListAsync();

            return contents.Select(c => new ContentDto
            {
                Id = c.Id,
                ContentKey = c.ContentKey,
                Name = language.ToLower() == "ar" ? c.NameAR : c.Name,
                NameAR = c.NameAR,
                Description = language.ToLower() == "ar" ? c.DescriptionAR : c.Description,
                DescriptionAR = c.DescriptionAR,
                MediaUrl = c.MediaUrl,
                IsActive = c.IsActive,
                SortOrder = c.SortOrder,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        // Additional methods required by the new interface
        public async Task<List<ContentDto>> GetContentBySectionAsync(string sectionKey)
        {
            var contents = await _context.WebsiteContents
                .Where(c => c.SectionKey == sectionKey && c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            return contents.Select(c => new ContentDto
            {
                Id = c.Id,
                ContentKey = c.ContentKey,
                Name = c.Name,
                NameAR = c.NameAR,
                Description = c.Description,
                DescriptionAR = c.DescriptionAR,
                MediaUrl = c.MediaUrl,
                IsActive = c.IsActive,
                SortOrder = c.SortOrder,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        public async Task<ContentDto> CreateContentAsync(CreateContentDto createDto)
        {
            // Check if content key already exists
            var existingContent = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.ContentKey == createDto.ContentKey);

            if (existingContent != null)
            {
                throw new ArgumentException("Content with this key already exists");
            }

            var content = new WebsiteContent
            {
                SectionKey = createDto.SectionKey,
                ContentKey = createDto.ContentKey,
                Name = createDto.Name,
                NameAR = createDto.NameAR,
                Description = createDto.Description,
                DescriptionAR = createDto.DescriptionAR,
                MediaUrl = createDto.MediaUrl,
                IsActive = createDto.IsActive,
                SortOrder = createDto.SortOrder,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.WebsiteContents.Add(content);
            await _context.SaveChangesAsync();

            return new ContentDto
            {
                Id = content.Id,
                ContentKey = content.ContentKey,
                Name = content.Name,
                NameAR = content.NameAR,
                Description = content.Description,
                DescriptionAR = content.DescriptionAR,
                MediaUrl = content.MediaUrl,
                IsActive = content.IsActive,
                SortOrder = content.SortOrder,
                CreatedAt = content.CreatedAt,
                UpdatedAt = content.UpdatedAt
            };
        }

        public async Task<ContentDto> UpdateContentAsync(int id, UpdateContentDto updateDto)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            // Check if content key is being changed and if it already exists
            if (!string.IsNullOrEmpty(updateDto.ContentKey) && updateDto.ContentKey != content.ContentKey)
            {
                var existingContent = await _context.WebsiteContents
                    .FirstOrDefaultAsync(c => c.ContentKey == updateDto.ContentKey && c.Id != id);

                if (existingContent != null)
                {
                    throw new ArgumentException("Content with this key already exists");
                }
                content.ContentKey = updateDto.ContentKey;
            }

            if (!string.IsNullOrEmpty(updateDto.SectionKey))
                content.SectionKey = updateDto.SectionKey;
            if (!string.IsNullOrEmpty(updateDto.Name))
                content.Name = updateDto.Name;
            if (!string.IsNullOrEmpty(updateDto.NameAR))
                content.NameAR = updateDto.NameAR;
            if (!string.IsNullOrEmpty(updateDto.Description))
                content.Description = updateDto.Description;
            if (!string.IsNullOrEmpty(updateDto.DescriptionAR))
                content.DescriptionAR = updateDto.DescriptionAR;
            if (!string.IsNullOrEmpty(updateDto.MediaUrl))
                content.MediaUrl = updateDto.MediaUrl;
            
            content.IsActive = updateDto.IsActive;
            content.SortOrder = updateDto.SortOrder;
            content.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ContentDto
            {
                Id = content.Id,
                ContentKey = content.ContentKey,
                Name = content.Name,
                NameAR = content.NameAR,
                Description = content.Description,
                DescriptionAR = content.DescriptionAR,
                MediaUrl = content.MediaUrl,
                IsActive = content.IsActive,
                SortOrder = content.SortOrder,
                CreatedAt = content.CreatedAt,
                UpdatedAt = content.UpdatedAt
            };
        }

        public async Task UpdateSortOrderAsync(int id, int sortOrder)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            content.SortOrder = sortOrder;
            content.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ToggleActiveStatusAsync(int id)
        {
            var content = await _context.WebsiteContents
                .FirstOrDefaultAsync(c => c.Id == id);

            if (content == null)
            {
                throw new ArgumentException("Content not found");
            }

            content.IsActive = !content.IsActive;
            content.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task BulkUpdateContentAsync(List<UpdateContentDto> updateDtos)
        {
            foreach (var updateDto in updateDtos)
            {
                if (updateDto.Id.HasValue)
                {
                    await UpdateContentAsync(updateDto.Id.Value, updateDto);
                }
            }
        }
    }
}
