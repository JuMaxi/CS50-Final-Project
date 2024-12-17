using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.DTO;

public class UserDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public DateOnly Birthday { get; set; }

    public string PostCode { get; set; }

    public static UserDTO FromUser(User user)
    {
        return new UserDTO()
        {
            Birthday = user.Birthday,
            Email = user.Email,
            FirstName = user.Name,
            Id = user.Id,
            LastName = user.LastName,
            PostCode = user.PostCode,
        };
    }
}
