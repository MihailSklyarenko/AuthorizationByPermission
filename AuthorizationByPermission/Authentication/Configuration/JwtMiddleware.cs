using AuthorizationByPermission.Helpers;
using AuthorizationByPermission.Interfaces;
using AuthorizationByPermission.Models.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthorizationByPermission.Authentication.Configuration;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtOptions _jwtSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<JwtOptions> jwtSettings)
    {
        _next = next;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var attachResult = await AttachUserToContextAsync(context, userService, token);
            if (attachResult == false)
            {
                return;
            }
        }

        await _next(context);
    }

    private async Task<bool> AttachUserToContextAsync(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            context.Items["User"] = await userService.GetByIdAsync(userId, context.RequestAborted);
            return true;
        }
        catch (SecurityTokenExpiredException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new ResponseBase
            {
                Success = false,
                Message = "Token expired."
            });

            return false;
        }
        catch
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new ResponseBase
            {
                Success = false,
                Message = "Server exception on work with token."
            });
            return false;
        }
    }
}
