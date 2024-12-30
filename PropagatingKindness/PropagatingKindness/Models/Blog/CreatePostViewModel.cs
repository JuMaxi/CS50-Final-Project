using System.ComponentModel.DataAnnotations;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Models.Blog
{
    public class CreatePostViewModel
    {
        public string ErrorMessage { get; set; } = string.Empty;
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Photo is required.")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "ShortDescription is required.")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Tag is required.")]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        public BlogDTO ConvertToDTO(string photoURL)
        {
            return new BlogDTO()
            {
                Id = Id,
                Title = Title,
                Photo = photoURL,
                ShortDescription = ShortDescription,
                Tags = Tags.Split(",").ToList(),
                Content = Content
            };
        }
    }
}
