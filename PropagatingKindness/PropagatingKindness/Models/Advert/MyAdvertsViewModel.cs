namespace PropagatingKindness.Models.Advert
{
    public class MyAdvertsViewModel
    {
        public List<AdvertViewModel> Adverts { get; set; } = [];

        public static MyAdvertsViewModel FromAdverts(List<Domain.Models.Advert> adverts)
        {
            return new MyAdvertsViewModel()
            {
                Adverts = adverts.Select(adv => AdvertViewModel.FromAdvert(adv)).ToList(),
            };
        }
    }

    public class AdvertViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }

        public int Status { get; set; }

        public static AdvertViewModel FromAdvert(PropagatingKindness.Domain.Models.Advert advert)

        {
            return new AdvertViewModel()
            {
               Name = advert.Name,
               Photo = advert.Photos.First().Location,
               Status = (int)advert.Status,
               Id = advert.Id,
            };
        }

    }
}

     
