using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
