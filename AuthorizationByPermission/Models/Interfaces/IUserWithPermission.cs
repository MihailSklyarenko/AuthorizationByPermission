namespace AuthorizationByPermission.Models.Interfaces;

public interface IUserWithPermission
{
    public ICollection<Permission.Permission> Permissions { get; set; }
}