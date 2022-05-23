using AuthorizationByPermission.Models.Interfaces;

namespace AuthorizationByPermission.Models
{
    public class User : IUserWithPermissions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ICollection<Permission.Permission> Permissions { get; set; }
    }
}
