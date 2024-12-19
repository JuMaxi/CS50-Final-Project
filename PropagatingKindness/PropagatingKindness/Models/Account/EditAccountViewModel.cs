using System.ComponentModel.DataAnnotations;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Account
{
    public class EditAccountViewModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(40, ErrorMessage = "First Name can't exceed 40 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(60, ErrorMessage = "Last Name can't exceed 600 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Birthday Date is required")]
        public DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "Post Code is required.")]
        [StringLength(7, ErrorMessage = "Post Code to the UK exceed 7 characters.")]
        public string PostCode { get; set; }

        public EditAccountViewModel FromUser(User user)
        {
            return new EditAccountViewModel()
            {
                //Photo = user.Photo,
                FirstName = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Birthday = user.Birthday,
                PostCode = user.PostCode,
            };
        }
        public UserDTO ConvertToDTO()
        {
            return new UserDTO()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Birthday = Birthday,
                PostCode = PostCode,
            };
        }


    }
}
