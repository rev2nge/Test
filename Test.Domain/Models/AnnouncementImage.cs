namespace Test.Domain.Models
{
    public class AnnouncementImage : EntityBase
    {
        public Guid AnnouncementId { get; set; }
        public byte[] OriginalImage { get; set; } = null!;
        public byte[] ThumbnailImage { get; set; } = null!;
        public string ImageFormat { get; set; } = null!;
    }
}
