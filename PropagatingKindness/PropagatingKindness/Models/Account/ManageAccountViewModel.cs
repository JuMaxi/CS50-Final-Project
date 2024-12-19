using System.ComponentModel.DataAnnotations;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Account
{
    public class ManageAccountViewModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        public IFormFile Photo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateOnly Birthday { get; set; }

        public string PostCode { get; set; }

        public ManageAccountViewModel FromUser(User user)
        {
            return new ManageAccountViewModel()
            {
                //Photo = user.Photo,
                FirstName = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Birthday = user.Birthday,
                PostCode = user.PostCode,
            };
        }
    }
}
