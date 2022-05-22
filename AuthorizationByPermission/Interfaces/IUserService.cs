using AuthorizationByPermission.Models;

namespace AuthorizationByPermission.Interfaces;

public interface IUserService
{
    Task<User> GetByIdAsync(int id, CancellationToken token);
    Task<User> GetByCredentialsAsync(string login, string password, CancellationToken token);
}
