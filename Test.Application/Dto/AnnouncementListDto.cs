namespace Test.Application.Dto
{
    public class AnnouncementListDto : EntityBaseDto
    {
        public int? Number { get; set; }
        public Guid? UserId { get; set; }
        public string? Text { get; set; }
        public string? Picture { get; set; }
        public string? Rate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
