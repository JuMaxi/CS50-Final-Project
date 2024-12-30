namespace PropagatingKindness.Domain.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailPhoto {  get; set; }
        public string CoverPhoto { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Date { get; set; }
        public List<Tag> Tags { get; set; } = [];
        public BlogPostContent Content {  get; set; }

        public void AddTag(string tag)
        {
            Tag t = new();
            t.Text = tag;
            Tags.Add(t);
        }
    }
}
