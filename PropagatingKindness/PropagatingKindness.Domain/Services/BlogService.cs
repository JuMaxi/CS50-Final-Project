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
            post.ThumbnailPhoto = postDTO.ThumbnailPhoto;
            post.CoverPhoto = postDTO.CoverPhoto;
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

        public async Task<BlogPost> GetPostById(int id)
        {
            return await _blogRepository.GetById(id);
        }

        private int CalculateAdvertToSkip(int page)
        {
            int skip = 0;

            if (page <= 0)
            {
                page = 1;
            }

            if (page > 1)
            {
                skip = 2 * (page - 1);
            }

            return skip;
        }

        public async Task<List<BlogPost>> GetAllPosts(int page)
        {
            int skip = CalculateAdvertToSkip(page);
            return await _blogRepository.GetAllPosts(skip);
        }

        public async Task<List<BlogPost>> GetSearchPosts(int page, string word)
        {
            int skip = CalculateAdvertToSkip(page);

            return await _blogRepository.GetSearchPosts(skip, word);
        }
        public async Task<int> GetCountAllPosts()
        {
            return await _blogRepository.GetCountAllPosts();
        }

        public async Task<int> GetCountSearchPosts(string word)
        {
            return await _blogRepository.GetCountSearchPosts(word);
        }
    }
}
