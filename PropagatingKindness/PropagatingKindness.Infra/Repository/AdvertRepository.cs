using Microsoft.EntityFrameworkCore;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;
using PropagatingKindness.Infra.Db;

namespace PropagatingKindness.Infra.Repository
{
    public class AdvertRepository : IAdvertRepository
    {
        private readonly PlantsDbContext _dbContext;

        public AdvertRepository(PlantsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Insert(Advert advert)
        {
            await _dbContext.AddAsync(advert);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Advert> GetById(int id)
        {
            return await _dbContext.Adverts.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Advert>> GetAll(int skip, int limit)
        {
            return await _dbContext.Adverts.Skip(skip).Take(limit).ToListAsync();
        }
        public async Task Update(Advert advert)
        {
            _dbContext.Adverts.Update(advert);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}
