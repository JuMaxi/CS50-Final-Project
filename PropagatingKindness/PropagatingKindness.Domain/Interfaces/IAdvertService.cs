using PropagatingKindness.Domain.DTO;

namespace PropagatingKindness.Domain.Interfaces
{
    public interface IAdvertService
    {
        public Task CreateAdvert(AdvertDTO advertDTO, int userId);
    }
}
