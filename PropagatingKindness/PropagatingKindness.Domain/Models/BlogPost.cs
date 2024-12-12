namespace PropagatingKindness.Domain.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Photo {  get; set; }
        public string ShortDescription { get; set; }
        public DateTime Date { get; set; }
        public List<Tag> Tags { get; set; }
        public BlogPostContent Content {  get; set; }
    }
}
