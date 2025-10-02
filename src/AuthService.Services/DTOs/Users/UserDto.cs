namespace AuthService.Services.DTOs.Users;

public class UserDto
{
    public string UserId { get; set; } = null!;
    
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;

    public bool IsActive { get; set; }

    public List<string> Roles { get; set; } = new();

    public List<string> Permissions { get; set; } = new();
}