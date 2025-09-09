using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TheNextEventAPI.DTOs;

namespace TheNextEventAPI.Services
{
    public interface ICloudinaryService
    {
        Task<UploadResultDto> UploadImageAsync(IFormFile file, string folder = "");
        Task<UploadResultDto> UploadVideoAsync(IFormFile file, string folder = "");
        Task<UploadResultDto> UploadFileAsync(IFormFile file, string folder = "");
        Task<bool> DeleteFileAsync(string publicId);
        Task<List<MediaFileDto>> GetAllFilesAsync(string folder = "");
        Task<string> GetOptimizedImageUrlAsync(string publicId, int? width = null, int? height = null);
        
        // Legacy methods
        CloudinarySignature GenerateSignature(string publicId);
        Task<bool> DeleteImageAsync(string publicId);
        Task<List<MediaItemDto>> GetAllImagesAsync();
    }

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;
            var cloudinarySection = configuration.GetSection("Cloudinary");
            var account = new Account(
                cloudinarySection["CloudName"],
                cloudinarySection["ApiKey"],
                cloudinarySection["ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        public CloudinarySignature GenerateSignature(string publicId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var parameters = new SortedDictionary<string, object>
            {
                { "public_id", publicId },
                { "timestamp", timestamp }
            };

            var signature = _cloudinary.Api.SignParameters(parameters);

            return new CloudinarySignature
            {
                Signature = signature,
                Timestamp = timestamp,
                ApiKey = _configuration.GetSection("Cloudinary")["ApiKey"] ?? string.Empty
            };
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                return result.Result == "ok";
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<MediaItemDto>> GetAllImagesAsync()
        {
            try
            {
                var result = await _cloudinary.ListResourcesAsync(new ListResourcesParams
                {
                    ResourceType = ResourceType.Image,
                    MaxResults = 100
                });

                return result.Resources.Select(r => new MediaItemDto
                {
                    PublicId = r.PublicId,
                    Url = r.Url?.ToString() ?? string.Empty,
                    SecureUrl = r.SecureUrl?.ToString() ?? string.Empty,
                    Format = r.Format ?? string.Empty,
                    ResourceType = r.ResourceType ?? string.Empty,
                    CreatedAt = DateTime.TryParse(r.CreatedAt, out var date) ? date : DateTime.MinValue,
                    Width = r.Width,
                    Height = r.Height,
                    Tags = r.Tags?.ToList() ?? new List<string>()
                }).ToList();
            }
            catch
            {
                return new List<MediaItemDto>();
            }
        }

        // New interface methods
        public async Task<UploadResultDto> UploadImageAsync(IFormFile file, string folder = "")
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new UploadResultDto
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url?.ToString() ?? string.Empty,
                SecureUrl = uploadResult.SecureUrl?.ToString() ?? string.Empty,
                Format = uploadResult.Format ?? string.Empty,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Bytes = uploadResult.Bytes,
                CreatedAt = uploadResult.CreatedAt
            };
        }

        public async Task<UploadResultDto> UploadVideoAsync(IFormFile file, string folder = "")
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new UploadResultDto
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url?.ToString() ?? string.Empty,
                SecureUrl = uploadResult.SecureUrl?.ToString() ?? string.Empty,
                Format = uploadResult.Format ?? string.Empty,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Bytes = uploadResult.Bytes,
                CreatedAt = uploadResult.CreatedAt
            };
        }

        public async Task<UploadResultDto> UploadFileAsync(IFormFile file, string folder = "")
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new UploadResultDto
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url?.ToString() ?? string.Empty,
                SecureUrl = uploadResult.SecureUrl?.ToString() ?? string.Empty,
                Format = uploadResult.Format ?? string.Empty,
                Bytes = uploadResult.Bytes,
                CreatedAt = uploadResult.CreatedAt
            };
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            return await DeleteImageAsync(publicId);
        }

        public async Task<List<MediaFileDto>> GetAllFilesAsync(string folder = "")
        {
            try
            {
                var listParams = new ListResourcesParams
                {
                    ResourceType = ResourceType.Image,
                    MaxResults = 100
                };

                if (!string.IsNullOrEmpty(folder))
                {
                    listParams.Type = folder;
                }

                var result = await _cloudinary.ListResourcesAsync(listParams);

                return result.Resources.Select(r => new MediaFileDto
                {
                    PublicId = r.PublicId,
                    Url = r.Url?.ToString() ?? string.Empty,
                    SecureUrl = r.SecureUrl?.ToString() ?? string.Empty,
                    Format = r.Format ?? string.Empty,
                    Width = r.Width,
                    Height = r.Height,
                    Bytes = r.Bytes,
                    CreatedAt = DateTime.TryParse(r.CreatedAt, out var createdDate) ? createdDate : DateTime.MinValue,
                    Folder = folder,
                    ResourceType = r.ResourceType ?? string.Empty
                }).ToList();
            }
            catch
            {
                return new List<MediaFileDto>();
            }
        }

        public async Task<string> GetOptimizedImageUrlAsync(string publicId, int? width = null, int? height = null)
        {
            try
            {
                var url = _cloudinary.Api.UrlImgUp.BuildUrl(publicId);
                
                if (width.HasValue || height.HasValue)
                {
                    var transformation = new Transformation();
                    if (width.HasValue) transformation.Width(width.Value);
                    if (height.HasValue) transformation.Height(height.Value);
                    transformation.Crop("fill");
                    
                    url = _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
                }

                return await Task.FromResult(url);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
