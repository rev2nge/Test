namespace Test.Domain.Models
{
    public class User : EntityBase
    {
        public string? Name { get; set; }
        public bool? Admin { get; set; }
    }
}
