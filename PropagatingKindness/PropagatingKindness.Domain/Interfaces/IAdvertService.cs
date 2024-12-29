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
        public Task<Result<Advert>> ActivateAdvert(int userId, int advertId);
        public Task<Result<Advert>> DonateAdvert(int userId, int advertId);
        public Task<Result<Advert>> PromisseAdvert(int userId, int advertId);
        public Task<Advert> GetAdvertById(int advertId);
        public Task<Result<List<Advert>>> GetAllAvailableAndPromissedAdverts(int page);
        public Task<int> GetCountAvailableAndPromissedAdverts();
        public Task<Result<List<Advert>>> SearchAvailableAndPromissedAdverts(int page, string word);
        public Task<int> SearchCountAvailableAndPromissedAdverts(string word);
    }
}
