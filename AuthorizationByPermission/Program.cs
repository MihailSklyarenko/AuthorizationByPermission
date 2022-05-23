using AuthorizationByPermission.Authentication.Configuration;
using AuthorizationByPermission.Authentication.Interfaces;
using AuthorizationByPermission.Authentication.Services;
using AuthorizationByPermission.Helpers;
using AuthorizationByPermission.Interfaces;
using AuthorizationByPermission.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt => 
    { 
        opt.JsonSerializerOptions.IgnoreNullValues = true; 
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<JwtMiddleware>();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();