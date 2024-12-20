using System.ComponentModel.DataAnnotations;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Account
{
    public class EditPasswordViewModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password can't exceed 100 characters.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password can't exceed 100 characters.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password can't exceed 100 characters.")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }

        public UserDTO ConvertToDTO()
        {
            return new UserDTO()
            {
               Password = CurrentPassword,
            };
        }

        public static EditPasswordViewModel FromUser(User user)

        {
            return new EditPasswordViewModel()
            {
                CurrentPassword = user.Password,
            };
        }

    }
    
}
