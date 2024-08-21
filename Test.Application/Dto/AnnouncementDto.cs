using System.ComponentModel.DataAnnotations;
using Test.Domain.Models;

namespace Test.Application.Dto
{
    public class AnnouncementDto : EntityBaseDto
    {
        public int Number { get; set; }
        public Guid? UserId { get; set; }
        [MaxLength(1000)]
        public string Text { get; set; }
        public AnnouncementImage? Picture { get; set; }
        [Range(1, 10)]
        public int? Rate { get; set; }
    }
}
