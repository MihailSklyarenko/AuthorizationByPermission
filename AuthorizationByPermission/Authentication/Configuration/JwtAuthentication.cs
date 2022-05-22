using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthorizationByPermission.Authentication.Configuration;

public static class JwtAuthentication
{
    public static void ConfigureJwtAuthentication(this IServiceCollection services)
    {
        var key = Encoding.ASCII.GetBytes("secretkeysecretkeysecretkeysecretkeysecretkey");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = "MyAuthServer",

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = "MyAuthClient",
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true,
                        };
                    });
    }
}