using AuthorizationByPermission.Helpers;
using AuthorizationByPermission.Interfaces;
using AuthorizationByPermission.Models.TokenResponse;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationByPermission.Authentication.Services;

public class AuthenticationService : Interfaces.IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly JwtOptions _JWTOptions;

    public AuthenticationService(IUserService userService, IOptions<JwtOptions> JWTOptions)
    {
        _userService = userService;
        _JWTOptions = JWTOptions.Value;
    }

    public async Task<TokenResponse> SignInAsync(string username, string password, CancellationToken token)
    {
        var identity = await GetIdentityAsync(username, password, token);
        if (identity == null)
        {
            return new TokenResponse() 
            { 
                Success = false,
                Message = "Invalid username or password!",
            };
        }

        var expire = _JWTOptions.ExpireInMinutes;
        var secret = _JWTOptions.Secret;
        var issuer = _JWTOptions.Issuer;
        var dateNow = DateTime.Now;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

        var jwt = new JwtSecurityToken(
                issuer: issuer,
                notBefore: dateNow,
                claims: identity.Claims,
                expires: dateNow.AddMinutes(expire),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));
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
