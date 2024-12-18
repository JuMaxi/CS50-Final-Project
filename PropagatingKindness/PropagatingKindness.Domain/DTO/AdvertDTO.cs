using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.DTO
{
    public class AdvertDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public int Status { get; set; }

        public static AdvertDTO FromAdvert(Advert advert)
        {
            List<string> p = new();
            foreach (var item in advert.Photos)
            {
                p.Add(item.ToString());
            }

            return new AdvertDTO
            {
                Id = advert.Id,
                UserId = advert.User.Id,
                Name = advert.Name,
                Description = advert.Description,
                Status = 1,
                Photos = p
            };
        }
    }
}
