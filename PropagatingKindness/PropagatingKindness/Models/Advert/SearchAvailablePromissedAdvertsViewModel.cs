namespace PropagatingKindness.Models.Advert
{
    public class SearchAvailablePromissedAdvertsViewModel
    {
        public List<SearchViewModel> Adverts { get; set; } = [];
        public int TotalAdverts { get; set; }
        public int CurrentPage { get; set; }
        public string KeyWord { get; set; }

        public static SearchAvailablePromissedAdvertsViewModel FromAdverts(List<Domain.Models.Advert> adverts, int total, int page, string word)
        {
            return new SearchAvailablePromissedAdvertsViewModel()
            {
                Adverts = adverts.Select(adv => SearchViewModel.FromAdvert(adv)).ToList(),
                TotalAdverts = total,
                CurrentPage = page,
                KeyWord = word
            };
        }
    }
    public class SearchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }

        public string PostCode { get; set; }
        public int Status { get; set; }

        public static SearchViewModel FromAdvert(PropagatingKindness.Domain.Models.Advert advert)

        {
            return new SearchViewModel()
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
