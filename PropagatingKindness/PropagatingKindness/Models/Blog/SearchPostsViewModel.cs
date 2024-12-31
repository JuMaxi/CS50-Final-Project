using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Blog
{
    public class SearchPostsViewModel
    {
        public List<SearchPost> ListPosts { get; set; } = [];
        public int TotalPosts { get; set; }
        public int CurrentPage { get; set; }

        public string Tag { get; set; }

        public static SearchPostsViewModel FromBlogPosts(List<BlogPost> posts, int total, int page, string tag)
        {
            return new SearchPostsViewModel()
            {
                ListPosts = posts.Select(p => SearchPost.FromBlogPost(p)).ToList(),
                TotalPosts = total,
                CurrentPage = page,
                Tag = tag
            };
        }
        public class SearchPost
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime PublicationDate { get; set; }
            public string ShortDescription { get; set; }
            public string ThumbnailPhoto { get; set; }


            public static SearchPost FromBlogPost(BlogPost blogPost)
            {
                return new SearchPost()
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
}
