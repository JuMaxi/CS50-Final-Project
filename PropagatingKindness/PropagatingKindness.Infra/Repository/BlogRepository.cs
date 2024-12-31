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
        
        public async Task<List<BlogPost>> GetSearchPosts(int skip, string tag)
        {
            return await _dbContext.Blogs
                .Include(a => a.Content)
                .Include(b => b.Tags)
                .Where(c => c.Tags.Any(t => t.Text == tag))
                .OrderByDescending(d => d.Date)
                .Skip(skip)
                .Take(2)
                .ToListAsync();
        }
        public async Task<int> GetCountAllPosts()
        {
            return await _dbContext.Blogs
                .CountAsync();
        }

        public async Task<int> GetCountSearchPosts(string tag)
        {
            return await _dbContext.Blogs
                .Include(a => a.Tags)
                .Where(b => b.Tags.Any(t => t.Text == tag))
                .CountAsync();
        }

        public async Task<List<BlogPost>> GetAllPosts(int skip, int take)
        {
            return await _dbContext.Blogs
                .Include(b => b.Content)
                .OrderByDescending(a => a.Date)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
