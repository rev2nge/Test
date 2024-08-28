using System.ComponentModel.DataAnnotations;

namespace Test.Application.Dto
{
    public class AnnouncementSearchDto : EntityBaseDto
    {
        public int? Number { get; set; }
        public Guid? UserId { get; set; }
        [MaxLength(1000)]
        public string? Text { get; set; }
        [Range(1, 10)]
        public int? Rate { get; set; }
        public DateTime? StartCreateDate { get; set; }  
        public DateTime? EndCreateDate { get; set; }
        public DateTime? StartExpiryDate { get; set; }
        public DateTime? EndExpiryDate { get; set; }
    }

}
