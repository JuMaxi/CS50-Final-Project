using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IAdvertRepository
    {
        public Task Insert(Advert advert);
        public Task<Advert> GetById(int id);
        public Task<List<Advert>> GetAll(int skip, int limit);
        public Task Update(Advert advert);
    }
}
