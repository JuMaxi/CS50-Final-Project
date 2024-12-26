using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly IAdvertRepository _advertRepository;
        private readonly IUserService _userService;

        public AdvertService(IAdvertRepository advertRepository, IUserService userService) 
        { 
            _advertRepository = advertRepository;
            _userService = userService;
        }

        public async Task CreateAdvert(AdvertDTO advertDTO, int userId)
        {
            User user = await _userService.GetById(userId);

            Advert advert = new()
            {
                User = user,
                Name = advertDTO.Name,
                Description = advertDTO.Description,
                Status = AdvertStatus.UnderReview,
                CreatedDate = DateTime.UtcNow,
            };

            foreach (string item in advertDTO.Photos)
                advert.AddPhoto(item);

            await _advertRepository.Insert(advert);
        }

        public async Task<List<Advert>> GetAllUserAdverts(int userId)
        {
            return await _advertRepository.GetAllUserAdverts(userId);
        }

        public async Task<List<Advert>> GetAllAdverts(int limit, int page)
        {
            int skip = 0;

            if (limit < 0 || limit > 1000)
            {
                limit = 10;
            }

            if (page < 0)
            {
                page = 1;
            }

            if (page > 1)
            {
                skip = limit * (page - 1);
            }

            return await _advertRepository.GetAllToSearch(skip, limit);
        }

        public async Task<List<Advert>> GetAllPendingAdverts()
        {
            return await _advertRepository.GetAllPendingAdverts();
        }

        public async Task UpdateAdvert(AdvertDTO advertDTO, int userId)
        {

        }
        public async Task<Result<Advert>> CheckUserOwnsAdvert(int userId, int advertId)
        {
            Advert advert = await GetAdvertById(advertId);

            if (advert is null || advert.User.Id != userId) 
            {
                return new Result<Advert>(false, string.Empty);
            }

            return new Result<Advert>(advert);
        }

        private async Task<Advert> GetAdvertById(int advertId)
        {
            return await _advertRepository.GetById(advertId);
        }
    }
}
