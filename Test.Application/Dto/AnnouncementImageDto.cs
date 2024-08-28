using Microsoft.AspNetCore.Http;

namespace Test.Application.Dto
{
    public class AnnouncementImageDto : EntityBaseDto
    {
        public string? ImagePath { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? ImageFormat { get; set; }
        public Guid AnnouncementId { get; set; }
        public IFormFile? File { get; set; }
    }
}