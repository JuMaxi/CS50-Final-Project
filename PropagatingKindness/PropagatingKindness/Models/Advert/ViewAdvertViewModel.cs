namespace PropagatingKindness.Models.Advert
{
    public class ViewAdvertViewModel
    {
        public int AdvertId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserPhoto {  get; set; }
        public string UserName { get; set; }
        public string UserPostCode { get; set; }
        public List<string> Photos { get; set; }

        public static ViewAdvertViewModel FromAdvert(Domain.Models.Advert advert)
        {
            return new ViewAdvertViewModel()
            {
                AdvertId = advert.Id,
                Name = advert.Name,
                Description = advert.Description,
                UserPhoto = advert.User.Photo,
                UserName = advert.User.Name,
                UserPostCode = advert.User.PostCode,
                Photos = advert.Photos.Select(p => p.Location).ToList()
            };
        }
    }
}
