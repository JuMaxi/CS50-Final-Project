namespace PropagatingKindness.Domain.Models
{
    public enum AdvertStatus
    {
        Available = 1, 
        Promissed = 2,
        Donated = 3,
        Inactive = 4
    }

    public class Advert
    {
        public int Id {  get; set; }
        public User User { get; set; }
        public string Name {  get; set; }
        public string Description { get; set; }
        public List<Photo> Photos { get; set; } = [];
        public AdvertStatus Status { get; set; }

        internal void AddPhoto(string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
                return;

            Photos.Add(new Photo() 
            {
                Location = photoUrl,
            });
        }
    }
}
