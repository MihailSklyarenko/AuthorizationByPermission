using AuthorizationByPermission.Models.Permission;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationByPermission.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeatherForecastController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize(Permission.EditForecast)]
    [HttpGet]
    public string Get()
    {
        var rnd = new Random();

        return rnd.Next(999).ToString();
    }
}