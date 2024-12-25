namespace PropagatingKindness.Models.Advert
{
    public class PendingAdvertsViewModel
    {
        public List<AdvertViewModel> Adverts { get; set; } = [];

        public static PendingAdvertsViewModel FromAdverts(List<Domain.Models.Advert> adverts)
        {
            return new PendingAdvertsViewModel()
            {
                Adverts = adverts.Select(adv => AdvertViewModel.FromAdvert(adv)).ToList(),
            };
        }
    }
}

     
