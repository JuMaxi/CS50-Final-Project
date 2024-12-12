namespace PropagatingKindness.Domain.Models
{
    public class GardeningHelp
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public string PostCode { get; set; }
        public string Description { get; set; }

    }
}
