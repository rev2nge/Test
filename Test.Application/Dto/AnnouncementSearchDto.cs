using Test.Domain.Models;

namespace Test.Application.Dto
{
    public class AnnouncementSearchDto : EntityBaseDto
    {
        public int Number { get; set; }
        public Guid? UserId { get; set; }
        public string Text { get; set; }
        public AnnouncementImage? Picture { get; set; }
        public int? Rate { get; set; }
        public DateTime? StartCreateDate { get; set; }  
        public DateTime? EndCreateDate { get; set; }
        public DateTime? StartExpiryDate { get; set; }
        public DateTime? EndExpiryDate { get; set; }
    }

}
