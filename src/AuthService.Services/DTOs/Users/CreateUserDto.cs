using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTOs.Users;

// this dto is only for the Auth service, so it is interested only in the credentials of the user
public class CreateUserDto
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_-]{3,15}$")]
    public string Username { get; set; } = null!;

    [Required]
    [RegularExpression(@"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+")]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$")]
    public string Password { get; set; } = null!;

    [Required]
    public List<string> Roles { get; set; } = null!;
    
    public List<string>? Permissions { get; set; }

}