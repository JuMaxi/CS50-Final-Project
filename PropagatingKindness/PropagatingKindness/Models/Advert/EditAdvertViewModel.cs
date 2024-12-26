using PropagatingKindness.Domain.DTO;
using System.ComponentModel.DataAnnotations;

namespace PropagatingKindness.Models.Advert
{
    public class EditAdvertViewModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "First Name can't exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string Description { get; set; }

        public int Status { get; set; }

        public AdvertDTO ConvertToDTO()
        {
            return new AdvertDTO
            {
                Name = Name,
                Description = Description,
                Status = Status,
            };
        }
        public static EditAdvertViewModel FromAdvert(PropagatingKindness.Domain.Models.Advert advert)
        {
            return new EditAdvertViewModel()
            {
                Name = advert.Name,
                Description = advert.Description
                // User can't change the photos. If it is needed, must Inactive the current advert and create a new one.
            };
        }
    }
}
