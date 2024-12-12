namespace PropagatingKindness.Domain.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Photo {  get; set; }
        public string ShortDescription { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; }
        public string Content {  get; set; }
    }
}
