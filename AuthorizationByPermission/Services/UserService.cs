using AuthorizationByPermission.Interfaces;
using AuthorizationByPermission.Models;
using AuthorizationByPermission.Models.Permission;

namespace AuthorizationByPermission.Services;

public class UserService : IUserService
{
    private readonly List<User> users = new List<User>
    {
        new User{ Id = 1, Name = "test", Login = "test", Password = "test" },
        new User{ Id = 2, Name = "test2", Login = "test2", Password = "test", Permissions = new List<Permission> { Permission.EditForecast } },
        new User{ Id = 3, Name = "test3", Login = "test3", Password = "test", Permissions = new List<Permission> { Permission.ViewForecast } },
        new User{ Id = 4, Name = "test4", Login = "test4", Password = "test", Permissions = new List<Permission> { Permission.EditForecast, Permission.ViewForecast } },
    };

    public Task<User> GetByIdAsync(int id, CancellationToken token)
    {
        return Task.FromResult(users[id]);
    }

    public Task<User> GetByCredentialsAsync(string login, string password, CancellationToken token)
    {
        var user = users.FirstOrDefault(x => x.Login == login && x.Password == password);
        return Task.FromResult(user);
    }
}
