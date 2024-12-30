using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Blog
{
    public class AllPostsViewModel
    {
        public List<Post> ListPosts { get; set; } = [];

        public static AllPostsViewModel FromBlogPosts(List<BlogPost> posts)
        {
            return new AllPostsViewModel()
            {
                ListPosts = posts.Select(p => Post.FromBlogPost(p)).ToList(),
            };
        }
        
    }
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailPhoto { get; set; }


        public static Post FromBlogPost(BlogPost blogPost)
        {
            return new Post()
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                PublicationDate = blogPost.Date,
                ShortDescription = blogPost.ShortDescription,
                ThumbnailPhoto = blogPost.ThumbnailPhoto,
            };
        }
    }
}
