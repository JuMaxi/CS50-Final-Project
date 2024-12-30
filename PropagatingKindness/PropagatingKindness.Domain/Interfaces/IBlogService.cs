using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IBlogService
    {
        public Task<Result> CreatePost(BlogDTO post, int userId);
        public Task<BlogPost> GetPostById(int id);
        public Task<List<BlogPost>> GetAllPosts();
    }
}
