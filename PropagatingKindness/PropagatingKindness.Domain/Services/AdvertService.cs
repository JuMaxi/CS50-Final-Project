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
            List<Photo> p = new();
            foreach (string item in advertDTO.Photos) 
            { 
                //p.Add((Photo)item);
            }

            User user = await _userService.GetById(userId);

            Advert advert = new()
            {
                User = user,
                Name = advertDTO.Name,
                Description = advertDTO.Description,
                Status = AdvertStatus.Available,
                Photos = p
            };
            await _advertRepository.Insert(advert);

        }





        public async Task<List<Advert>> GetAll(int limit, int page)
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

            return await _advertRepository.GetAll(skip, limit);
        }
    }
}
