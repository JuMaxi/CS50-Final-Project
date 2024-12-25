using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Domain.Interfaces;
using System.Security.Claims;

namespace PropagatingKindness.Services;

public class EnsureUserImageActionFilter : IAsyncActionFilter
{
    private readonly IUserRepository _userRepository;

    public EnsureUserImageActionFilter(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controller = context.Controller as Controller;
        if (controller != null)
        {
            if (controller.HttpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                var userId = Convert.ToInt32(controller.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var user = await _userRepository.GetById(userId);
                controller.ViewBag.UserAvatar = user.Photo;
            }
        }

        await next();
    }
}