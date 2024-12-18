using PropagatingKindness.Domain.Interfaces;
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
    }
}
