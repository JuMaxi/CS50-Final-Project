using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IAdvertService
    {
        public Task CreateAdvert(AdvertDTO advertDTO, int userId);
        public Task<List<Advert>> GetAllUserAdverts(int userId);
        public Task<List<Advert>> GetAllPendingAdverts();
        public Task<Result<Advert>> CheckUserOwnsAdvert(int userId, int advertId);
        public Task<Result<Advert>> UpdateAdvert(AdvertDTO advertDTO);
        public Task<Result<Advert>> DeactivateAdvert(int userId, int advertId);
    }
}
