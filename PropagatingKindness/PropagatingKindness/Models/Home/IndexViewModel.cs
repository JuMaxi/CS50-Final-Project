using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Home
{
    public class IndexViewModel
    {
        public List<CarrousselBlogPost> Posts { get; set; } = [];

        public static IndexViewModel FromBlogPosts(List<BlogPost> posts) 
        {
            return new IndexViewModel()
            {
                Posts = posts.Select(p => CarrousselBlogPost.FromBlogPost(p)).ToList(),
            };
        }
    }

    public class CarrousselBlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Photo { get; set; }

        public static CarrousselBlogPost FromBlogPost(BlogPost blogPost)
        {
            return new CarrousselBlogPost()
            {
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Photo = blogPost.CoverPhoto,
                Id = blogPost.Id
            };
        }
    }
}
