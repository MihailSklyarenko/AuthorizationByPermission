using AuthorizationByPermission.Models.Interfaces;
using AuthorizationByPermission.Models.Permission;
using AuthorizationByPermission.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly List<Permission> _permissions;

    public AuthorizeAttribute(Permission mainPermission, params Permission[] otherPermissions)
    {
        _permissions = new List<Permission>(otherPermissions) { mainPermission };
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (IUserWithPermissions)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new ResponseBase 
            { 
                Success = false, 
                Message = "Unauthorized." 
            }) 
            { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        if (user.Permissions is null || user.Permissions.Intersect(_permissions).Any() == false)
        {
            context.Result = new JsonResult(new ResponseBase
            {
                Success = false,
                Message = "You do not have access to this action."
            }) 
            { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }
    }
}