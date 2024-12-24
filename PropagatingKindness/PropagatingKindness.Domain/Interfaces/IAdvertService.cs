using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IAdvertService
    {
        public Task CreateAdvert(AdvertDTO advertDTO, int userId);
        public Task<List<Advert>> GetAllUserAdverts(int userId);
    }
}
