﻿using System.ComponentModel.DataAnnotations;

namespace Test.Domain.Models
{
    public class Announcement : EntityBase
    {
        public int Number { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string Text { get; set; }
        public AnnouncementImage? Picture { get; set; }
        public int? Rate { get; set; }
        public DateTime CreateDate { get; set; } 
        public DateTime ExpiryDate { get; set; }
    }
}