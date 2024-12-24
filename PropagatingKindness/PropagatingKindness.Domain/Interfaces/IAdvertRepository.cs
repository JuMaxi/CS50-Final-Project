using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IAdvertRepository
    {
        public Task Insert(Advert advert);
        public Task<Advert> GetById(int id);
        public Task<List<Advert>> GetAllToSearch(int skip, int limit);
        public Task Update(Advert advert);

        public Task<List<Advert>> GetAllUserAdverts(int userId);
    }
}
