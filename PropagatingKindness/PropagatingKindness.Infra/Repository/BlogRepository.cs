using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;
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
        public async Task<List<BlogPost>> GetAllPosts(int skip)
        {
            return await _dbContext.Blogs
                .Include(b => b.Content)
                .Skip(skip)
                .Take(2)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
        public async Task<List<BlogPost>> GetSearchPosts(int skip, string word)
        {
            return await _dbContext.Blogs
                .Include(b => b.Content)
                .Where(b => b.Title.Contains(word))
                .Skip(skip)
                .Take(2)
                .OrderByDescending(d => d.Date)
                .ToListAsync();
        }
        public async Task<int> GetCountAllPosts()
        {
            return await _dbContext.Blogs
                .CountAsync();
        }

        public async Task<int> GetCountSearchPosts(string word)
        {
            return await _dbContext.Blogs
                .Where(b => b.Title.Contains(word))
                .CountAsync();
        }
    }
}
