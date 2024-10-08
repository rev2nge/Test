﻿namespace Test.Domain.Models
{
    public class AnnouncementImage : EntityBase
    {
        public string? ImagePath { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? ImageFormat { get; set; }
        public Guid AnnouncementId { get; set; }
        public Announcement Announcement { get; set; }
    }
}
