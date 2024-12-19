using Microsoft.EntityFrameworkCore;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;
using PropagatingKindness.Infra.Db;

namespace PropagatingKindness.Infra.DbAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly PlantsDbContext _dbContext;

        public UserRepository(PlantsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Insert(User user)
        {
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByEmail(string email)
        {
            var result = await _dbContext.Users.Where(e => e.Email == email.ToLower()).FirstOrDefaultAsync();
            return result;
        }

        public async Task<User> GetById(int id)
        {
            return await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
