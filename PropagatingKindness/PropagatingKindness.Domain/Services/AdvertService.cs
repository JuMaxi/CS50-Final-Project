﻿using PropagatingKindness.Domain.DTO;
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
        private async Task<Advert> GetAdvertById(int advertId)
        {
            return await _advertRepository.GetById(advertId);
        }

        public async Task<Result<Advert>> DeactivateAdvert(int userId, int advertId)
        {
            Result<Advert> advert = await CheckUserOwnsAdvert(userId, advertId);

            if (advert.Success && advert.Content.Status is not AdvertStatus.Donated) 
            {
                advert.Content.Status = AdvertStatus.Inactive;

                await _advertRepository.Update(advert.Content);

                return advert;
            }
            return advert;
        }
    }
}
