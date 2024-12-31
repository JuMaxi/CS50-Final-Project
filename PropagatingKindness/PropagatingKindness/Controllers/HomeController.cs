using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Models.Home;

namespace PropagatingKindness.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public HomeController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await _blogRepository.GetAllPosts(skip: 0, take: 8);
            return View(IndexViewModel.FromBlogPosts(blogPosts));
        }
    }
}
