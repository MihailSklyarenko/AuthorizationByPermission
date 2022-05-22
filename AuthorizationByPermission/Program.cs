using AuthorizationByPermission.Authentication.Configuration;
using AuthorizationByPermission.Authentication.Interfaces;
using AuthorizationByPermission.Authentication.Services;
using AuthorizationByPermission.Helpers;
using AuthorizationByPermission.Interfaces;
using AuthorizationByPermission.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o => { o.JsonSerializerOptions.IgnoreNullValues = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpContextAccessor();
//builder.Services.ConfigureJwtAuthentication();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
