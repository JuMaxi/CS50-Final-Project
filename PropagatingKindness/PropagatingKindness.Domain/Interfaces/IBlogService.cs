using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IBlogService
    {
        public Task<Result> CreatePost(BlogDTO post, int userId);
        public Task<BlogPost> GetPostById(int id);
        public Task<List<BlogPost>> GetAllPosts(int page);
        public Task<List<BlogPost>> GetSearchPosts(int page, string word);
        public Task<int> GetCountAllPosts();
        public Task<int> GetCountSearchPosts(string word);
    }
}
