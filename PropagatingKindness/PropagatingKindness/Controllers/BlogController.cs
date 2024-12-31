using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Services;
using PropagatingKindness.Models.Blog;
using PropagatingKindness.Services;

namespace PropagatingKindness.Controllers
{
    public class BlogController : Controller
    {
        private readonly IReCaptchaService _reCaptchaService;
        private readonly IBlogService _blogService;
        private readonly IPhotosManagerService _photosService;

        public BlogController(
            IReCaptchaService reCaptchaService,
            IBlogService blogService,
            IPhotosManagerService photosService)
        {
            _reCaptchaService = reCaptchaService;
            _blogService = blogService;
            _photosService = photosService;
        }

        private int GetUserId()
        {
            return Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

        [HttpGet]
        public async Task<IActionResult> All(int page)
        {
            if (page == 0)
            {
                page = 1;
            }

            int count = await _blogService.GetCountAllPosts();

            var posts = await _blogService.GetAllPosts(page);

            return View(AllPostsViewModel.FromBlogPosts(posts, count, page));
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [RequiresAdmin]
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostViewModel postView, IFormCollection form)
        {
            if (string.IsNullOrWhiteSpace(form["g-recaptcha-response"]))
            {
                postView.ErrorMessage = "Please solve the captcha challenge";
                return View(postView);
            }

            if (ModelState.IsValid)
            {
                var recaptcha = await _reCaptchaService.ValidateRecaptcha(form["g-recaptcha-response"]);
                if (!recaptcha.Success)
                {
                    postView.ErrorMessage = recaptcha.ErrorMessage;
                    return View(postView);
                }

                var imagePath = await _photosService.ResizeAndUpload(postView.ThumbnailPhoto, maxWidth: 500, maxHeight: 500, blobContainer: "blogs");
                var imagePathCover = await _photosService.ResizeAndUpload(postView.CoverPhoto, maxWidth: 1500, maxHeight: 500, blobContainer: "blogs");

                var dto = postView.ConvertToDTO(imagePath, imagePathCover);

                var result = await _blogService.CreatePost(dto, GetUserId());

                if (result.Success)
                    return View();
            }
            return RedirectToAction("Home", "Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(int id)
        {
            var post = await _blogService.GetPostById(id);
            return View(ViewPostViewModel.FromBlogPost(post));
        }
    }
}
