using AuthorizationByPermission.Models.TokenResponse;

namespace AuthorizationByPermission.Authentication.Interfaces;

public interface IAuthenticationService
{
    Task<TokenResponse> SignInAsync(string username, string password, CancellationToken ct);
}