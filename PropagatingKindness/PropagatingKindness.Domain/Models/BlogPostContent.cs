namespace PropagatingKindness.Domain.Models
{
    public class BlogPostContent
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
