namespace AuthService.DAL.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public byte[] HashedPassword { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
