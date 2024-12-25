namespace PropagatingKindness.Domain.Models;

public enum AccessLevel
{
    Disabled = 0,
    User = 1,
    Moderator = 2,
    Admin = 3,
}

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
    public AccessLevel AccessLevel { get; set; }
    
}
