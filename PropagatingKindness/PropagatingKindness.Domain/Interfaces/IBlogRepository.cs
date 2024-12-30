using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IBlogRepository
    {
        public Task Insert(BlogPost blogPost);
        public Task<BlogPost> GetById(int id);
        public Task<List<BlogPost>> GetAllPosts();
    }
}
