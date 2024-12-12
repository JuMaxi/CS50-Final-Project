namespace PropagatingKindness.Domain.Models
{
    public class Gardening_Help
    {
        public int Id {  get; set; }
        public User User { get; set; }
        public string PostCode { get; set; }
        public string Description { get; set; }

    }
}
