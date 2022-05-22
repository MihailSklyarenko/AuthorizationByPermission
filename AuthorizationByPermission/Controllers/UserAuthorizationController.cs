using AuthorizationByPermission.Authentication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationByPermission.Controllers;

[ApiController]
[Route("[controller]")]
public class UserAuthorizationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public UserAuthorizationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }


    [HttpGet]
    public async Task<IActionResult> LogIn(string username, string password, CancellationToken token)
    {
        var result = await _authenticationService.SignInAsync(username, password, token);
        return Ok(result);
    }
}