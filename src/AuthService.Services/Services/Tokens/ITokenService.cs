using AuthService.DAL.Models;

namespace AuthService.Services.Services.Tokens;

public interface ITokenService
{
    public string GenerateToken(string username, string email, List<Role> roles, List<Permission> permissions);
}