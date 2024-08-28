namespace Test.Application.Dto
{
    public class AnnouncementListDto : EntityBaseDto
    {
        public int Number { get; set; }
        public Guid? UserId { get; set; }
        public string Text { get; set; }
        public int? Rate { get; set; }
        public ICollection<AnnouncementImageDto> Images { get; set; }
    }
}
