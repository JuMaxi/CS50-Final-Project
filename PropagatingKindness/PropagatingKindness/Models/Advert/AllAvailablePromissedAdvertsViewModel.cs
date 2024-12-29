namespace PropagatingKindness.Models.Advert
{
    public class AllAvailablePromissedAdvertsViewModel
    {
        public List<AllViewModel> Adverts { get; set; } = [];

        public static AllAvailablePromissedAdvertsViewModel FromAdverts(List<Domain.Models.Advert> adverts)
        {
            return new AllAvailablePromissedAdvertsViewModel()
            {
                Adverts = adverts.Select(adv => AllViewModel.FromAdvert(adv)).ToList(),
            };
        }
    }

    public class AllViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }

        public string PostCode { get; set; }
        public int Status { get; set; }

        public static AllViewModel FromAdvert(PropagatingKindness.Domain.Models.Advert advert)

        {
            return new AllViewModel()
            {
                Name = advert.Name,
                Photo = advert.Photos.First().Location,
                Status = (int)advert.Status,
                PostCode = advert.User.PostCode.Substring(0, advert.User.PostCode.Length - 3),
                Id = advert.Id,
            };
        }
    }
}
