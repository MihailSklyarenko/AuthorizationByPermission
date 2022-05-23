namespace AuthorizationByPermission.Models.Interfaces;

public interface IUserWithPermissions
{
    public ICollection<Permission.Permission> Permissions { get; set; }
}