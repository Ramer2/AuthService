namespace AuthService.DAL.Models;

public partial class Permission
{
    public Guid PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
