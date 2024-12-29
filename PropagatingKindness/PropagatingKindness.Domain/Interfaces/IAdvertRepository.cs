using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IAdvertRepository
    {
        public Task Insert(Advert advert);
        public Task<Advert> GetById(int id);
        public Task Update(Advert advert);
        public Task<List<Advert>> GetAllUserAdverts(int userId);
        public Task<List<Advert>> GetAllPendingAdverts();
        public Task<List<Advert>> GetAllAvailableAndPromissedAdverts(int page);
    }
}
