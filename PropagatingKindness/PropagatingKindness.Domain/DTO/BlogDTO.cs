using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.DTO
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; }
        public string Content { get; set; }

        public static BlogDTO FromBlogPost(BlogPost post)
        {
            List<string> tags = new List<string>(); 
            foreach (var item in post.Tags) 
            { 
                tags.Add(item.ToString());
            }
            
            return new BlogDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Photo = post.Photo,
                ShortDescription = post.ShortDescription,
                Date = post.Date,
                Content = post.Content.Content,
                Tags = tags
            };
        }
    }
}
