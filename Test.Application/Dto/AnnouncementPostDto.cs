﻿using System.ComponentModel.DataAnnotations;

namespace Test.Application.Dto
{
    public class AnnouncementPostDto : EntityBaseDto
    {
        public int Number { get; set; }
        public Guid? UserId { get; set; }
        [MaxLength(1000)]
        public string Text { get; set; }
        [Range(1, 10)]
        public int? Rate { get; set; }
    }
}
