namespace AuthorizationByPermission.Helpers;

public class JwtOptions
{
    public string Secret { get; set; }
    public int ExpireInMinutes { get; set; }
    public string Issuer { get; set; }
}
