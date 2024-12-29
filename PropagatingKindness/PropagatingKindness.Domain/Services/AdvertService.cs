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

        private int CalculateAdvertToSkip(int page)
        {
            int skip = 0;

            if (page <= 0)
            {
                page = 1;
            }

            if (page > 1)
            {
                skip = 16 * (page - 1);
            }

            return skip;
        }

        public async Task<Result<List<Advert>>> GetAllAvailableAndPromissedAdverts(int page)
        {
            int skip = CalculateAdvertToSkip(page);
            List<Advert> adverts = await _advertRepository.GetAllAvailableAndPromissedAdverts(skip);

            return new Result<List<Advert>> (adverts);
        }

        public async Task<List<Advert>> GetAllPendingAdverts()
        {
            return await _advertRepository.GetAllPendingAdverts();
        }
       
        public async Task<Result<Advert>> CheckUserOwnsAdvert(int userId, int advertId)
        {
            Advert advert = await GetAdvertById(advertId);
            User user = await _userService.GetById(userId);

            if (advert is null || (advert.User.Id != userId && !user.IsAdmin)) 
            {
                return new Result<Advert>(false, string.Empty);
            }

            return new Result<Advert>(advert);
        }
        public async Task<Result<Advert>> UpdateAdvert(AdvertDTO advertDTO)
        {
            Result<Advert> advert = await CheckUserOwnsAdvert(advertDTO.UserId, advertDTO.Id);

            if (advert.Success) 
            {
                advert.Content.Name = advertDTO.Name;
                advert.Content.Description = advertDTO.Description;
                advert.Content.Status = 0;

                await _advertRepository.Update(advert.Content);

                return advert;
            };
            return advert;
        }
        public async Task<Advert> GetAdvertById(int advertId)
        {
            return await _advertRepository.GetById(advertId);
        }
        
        public async Task<Result<Advert>> DeactivateAdvert(int userId, int advertId)
        {
            Result<Advert> advert = await CheckUserOwnsAdvert(userId, advertId);

            User user = await _userService.GetById(userId);

            if (advert.Success) 
            {
                if (user.IsAdmin || advert.Content.Status == AdvertStatus.Available) 
                {
                    advert.Content.Status = AdvertStatus.Inactive;

                    await _advertRepository.Update(advert.Content);
                }
                else
                {
                    // Making success false to return the user for the home page instead of my adverts. This function
                    //      is called at advertController
                    advert.Success = false;
                }
            }
            return advert;
        }

        public async Task<Result<Advert>> ActivateAdvert(int userId, int advertId)
        {
            Result<Advert> advert = await CheckUserOwnsAdvert(userId, advertId);

            User user = await _userService.GetById(userId);

            if (advert.Success) 
            {
                if (user.IsAdmin || advert.Content.Status == AdvertStatus.Promissed) 
                { 
                    advert.Content.Status = AdvertStatus.Available;

                    await _advertRepository.Update(advert.Content);
                }
                else
                {
                    advert.Success = false;
                }
            }
            return advert;
        }

        public async Task<Result<Advert>> DonateAdvert(int userId, int advertId)
        {
            Result<Advert> advert = await CheckUserOwnsAdvert(userId, advertId);

            User user = await _userService.GetById(userId);

            List<AdvertStatus> allowedStatus = [AdvertStatus.Promissed, AdvertStatus.Available];

            if (advert.Success) 
            {
                if (user.IsAdmin || allowedStatus.Contains(advert.Content.Status))
                {
                    advert.Content.Status = AdvertStatus.Donated;

                    await _advertRepository.Update(advert.Content);
                }
                else
                {
                    advert.Success = false;
                }
            }
            return advert;
        }

        public async Task<Result<Advert>> PromisseAdvert(int userId, int advertId)
        {
            Result<Advert> advert = await CheckUserOwnsAdvert(userId,advertId);

            User user = await _userService.GetById(userId);

            if (advert.Success)
            {
                if (user.IsAdmin || advert.Content.Status == AdvertStatus.Available)
                {
                    advert.Content.Status = AdvertStatus.Promissed;

                    await _advertRepository.Update(advert.Content);
                }
                else
                {
                    advert.Success = false;
                }
            }

            return advert;
        }

        public async Task<Result<Advert>> DisplayAdvert(int advertId)
        {
            // When the user select an advert, this method will display the advert

            return new Result<Advert>(false, string.Empty);
        }
    }
}
