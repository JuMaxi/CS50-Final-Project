using Microsoft.EntityFrameworkCore;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;
using PropagatingKindness.Infra.Db;

namespace PropagatingKindness.Infra.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly PlantsDbContext _dbContext;
        public BlogRepository(PlantsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Insert(BlogPost blogPost)
        {
            await _dbContext.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<BlogPost> GetById(int id)
        {
            return await _dbContext.Blogs
                .Include(b => b.Content)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<List<BlogPost>> GetAllPosts()
        {
            return await _dbContext.Blogs
                .Include(b => b.Content)
                .ToListAsync();
        }
    }
}
