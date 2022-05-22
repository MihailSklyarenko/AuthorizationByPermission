using AuthorizationByPermission.Models.Interfaces;
using AuthorizationByPermission.Models.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly Permission[] _permissions;
    public AuthorizeAttribute(params Permission[] permissions)
    {
        _permissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (IUserWithPermission)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        if (user.Permissions.Intersect(_permissions).Any() == false)
        {
            context.Result = new JsonResult(new { message = "You do not have access to this action" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }
    }
}