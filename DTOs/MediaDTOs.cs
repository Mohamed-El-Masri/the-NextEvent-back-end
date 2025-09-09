namespace TheNextEventAPI.DTOs
{
    public class UploadResultDto
    {
        public string PublicId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string SecureUrl { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public long Bytes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class MediaFileDto
    {
        public string PublicId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string SecureUrl { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public long Bytes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Folder { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
    }

    public class MediaItemDto
    {
        public string PublicId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string SecureUrl { get; set; } = string.Empty;
        public string OriginalFilename { get; set; } = string.Empty;
        public long Size { get; set; }
        public string Format { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }

    public class CloudinarySignature
    {
        public string Signature { get; set; } = string.Empty;
        public long Timestamp { get; set; }
        public string ApiKey { get; set; } = string.Empty;
    }
}