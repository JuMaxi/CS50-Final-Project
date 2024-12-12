namespace PropagatingKindness.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Photo {  get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly Birthday { get; set; }
        public string PostCode { get; set; }
        
    }
}
