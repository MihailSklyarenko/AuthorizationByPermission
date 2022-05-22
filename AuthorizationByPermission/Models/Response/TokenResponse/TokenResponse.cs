using AuthorizationByPermission.Models.Response;

namespace AuthorizationByPermission.Models.TokenResponse;

public class TokenResponse : ResponseBase
{
    public string AccessToken { get; set; }
    public string Username { get; set; }
}
