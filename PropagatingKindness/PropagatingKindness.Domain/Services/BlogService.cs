using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUserRepository _userRepository;

        public BlogService(IBlogRepository blogRepository, IUserRepository userRepository)
        {
            _blogRepository = blogRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> CreatePost(BlogDTO postDTO, int userId)
        {
            User user = await _userRepository.GetById(userId);

            if (!user.IsAdmin) 
            {
                return new Result(false, string.Empty);
            }

            BlogPost post = new();

            foreach (var tag in postDTO.Tags) 
            { 
                post.AddTag(tag);
            }

            post.Date = DateTime.Now;
            post.Title = postDTO.Title;
            post.Photo = postDTO.Photo;
            post.ShortDescription = postDTO.ShortDescription;

            BlogPostContent content = new()
            {
                Content = postDTO.Content,
                BlogPost = post,
            };
            post.Content = content;

            await _blogRepository.Insert(post);
            return new Result(true, string.Empty);
        }
    }
}
