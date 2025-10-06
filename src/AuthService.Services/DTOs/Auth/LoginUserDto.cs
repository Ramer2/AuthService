using System.ComponentModel.DataAnnotations;

namespace AuthService.Services.DTOs.Auth;

public class LoginUserDto
{
    // this can be either email or username, the api will search both
    [Required]
    public string Login { get; set; }
    
    [Required]
    public string Password { get; set; }
}