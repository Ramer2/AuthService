using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTOs.Users;

// this dto is only for the Auth service, so it is interested only in the credentials of the user
public class CreateUserDto
{
    [Required]
    [RegularExpression("@^[a-zA-Z0-9_-]{3,15}$")]
    public string Username { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$")]
    public string Password { get; set; } = null!;
    
}