﻿using System.ComponentModel.DataAnnotations;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Advert
{
    public class CreateAdvertViewModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "First Name can't exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Photo is required.")]
        public IFormFile Photo1 { get; set; }
        public IFormFile Photo2 { get; set; }
        public IFormFile Photo3 { get; set; }
        public IFormFile Photo4 { get; set; }

        public int Status { get; set; }

        public AdvertDTO ConvertToDTO()
        {
            List<string> p = new();
            p.Add(Photo1.ToString());
            p.Add(Photo2?.ToString());
            p.Add(Photo3?.ToString());
            p.Add(Photo4?.ToString());

            return new AdvertDTO
            {
                Name = Name,
                Description = Description,
                Status = Status,
                Photos = p
            };
        }
    }
}