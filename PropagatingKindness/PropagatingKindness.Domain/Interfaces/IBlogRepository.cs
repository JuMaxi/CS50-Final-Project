using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IBlogRepository
    {
        public Task Insert(BlogPost blogPost);
        public Task<BlogPost> GetById(int id);
        public Task<List<BlogPost>> GetAllPosts(int skip);
        public Task<List<BlogPost>> GetSearchPosts(int skip, string word);
        public Task<int> GetCountAllPosts();
        public Task<int> GetCountSearchPosts(string word);
    }
}
