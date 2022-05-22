namespace AuthorizationByPermission.Models.Response;

public class ResponseBase
{
    public bool Success { get; set; } = true;
    public string Message { get; set; }
}
