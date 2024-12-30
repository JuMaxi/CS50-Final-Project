using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Blog
{
    public class ViewPostViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<string> Tags { get; set; } = [];
        public string Content { get; set; }

        public static ViewPostViewModel FromBlogPost(BlogPost blogPost)
        {
            List<string> t = new List<string>();
            foreach (var item in blogPost.Tags) 
            { 
                t.Add(item.ToString());
            }
            return new ViewPostViewModel()
            {
                PostId = blogPost.Id,
                Title = blogPost.Title,
                Photo = blogPost.Photo,
                PublicationDate = blogPost.Date,
                Tags = t,
                Content = blogPost.Content.Content
            };
        }
    }
}
