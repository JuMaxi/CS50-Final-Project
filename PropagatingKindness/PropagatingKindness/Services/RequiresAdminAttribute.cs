using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Services;

public class RequiresAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext?.User;

        if (!IsAuthenticated(user))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Check if the user has access
        var accessLevel = Convert.ToInt32(user.Claims.First(x => x.Type == ClaimTypes.GroupSid).Value); 
        if (accessLevel != (int)AccessLevel.Admin)
        {
            context.Result = new ForbidResult();
            return;
        }
    }

    private bool IsAuthenticated(ClaimsPrincipal user) => user?.Identity?.IsAuthenticated ?? false;
}
