namespace Test.Application.Dto
{
    public class AnnouncementDto : EntityBaseDto
    {
        public int Number { get; set; }
        public Guid? UserId { get; set; }
        public string Text { get; set; }
        public int? Rate { get; set; }
        public ICollection<AnnouncementImageDto> Images { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
