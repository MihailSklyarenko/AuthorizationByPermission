using AuthorizationByPermission.Interfaces;
using AuthorizationByPermission.Models.TokenResponse;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationByPermission.Authentication.Services;

public class AuthenticationService : Interfaces.IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    public async Task<TokenResponse> SignInAsync(string username, string password, CancellationToken token)
    {
        var identity = await GetIdentityAsync(username, password, token);
        if (identity == null)
        {
            return new TokenResponse() 
            { 
                Message = "Invalid username or password!",
                Success = false,
            };
        }

        var now = DateTime.Now;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
                issuer: "MyAuthServer",
                audience: "MyAuthClient",
                notBefore: now,
                claims: identity.Claims,
                expires: now.AddMinutes(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("secretkeysecretkeysecretkeysecretkeysecretkey")), 
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new TokenResponse()
        {
            AccessToken = encodedJwt,
            Username = username,
        };
    }

    private async Task<ClaimsIdentity> GetIdentityAsync(string username, string password, CancellationToken token)
    {
        var user = await _userService.GetByCredentialsAsync(username, password, token);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            return claimsIdentity;
        }

        return null;
    }
}
