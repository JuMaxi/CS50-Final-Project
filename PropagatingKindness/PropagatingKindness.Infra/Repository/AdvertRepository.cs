﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
            return await _dbContext.Adverts
                .Include(p => p.Photos)
                .Include(u => u.User)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Advert advert)
        {
            _dbContext.Adverts.Update(advert);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Advert>> GetAllUserAdverts(int userId)
        {
            return await _dbContext.Adverts.Include(p => p.Photos).Where(u => u.User.Id == userId).ToListAsync();
        }

        public async Task<List<Advert>> GetAllPendingAdverts()
        {
            return await _dbContext.Adverts.Include(a => a.Photos)
                .Where(x => x.Status == AdvertStatus.UnderReview)
                .ToListAsync();
        }

        public async Task<List<Advert>> GetAllAvailableAndPromissedAdverts(int page)
        {
            return await _dbContext.Adverts.Include(p => p.Photos)
                .Include(u => u.User)
                .Where(a => a.Status == AdvertStatus.Available || a.Status == AdvertStatus.Promissed)
                .Skip(page)
                .Take(16)
                .ToListAsync();
        }
    }
}
