using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.DAL.Models;
using AuthService.Services.Helpers.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services.Services.Tokens;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly RsaSecurityKey _rsaSecurityKey;
    private readonly RSA _rsa;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
        
        _rsa = RSA.Create();
        _rsa.ImportFromPem(File.ReadAllText(_jwtOptions.PrivateKeyPath));
        
        _rsaSecurityKey = new RsaSecurityKey(_rsa);
    }
    
    public string GenerateToken(string username, string email, List<Role> roles, List<Permission> permissions)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, username),
            new("username", username),
            new(ClaimTypes.Email, email)
        };
        
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.RoleName)));

        claims.AddRange(permissions.Select(p => new Claim("permission", p.PermissionName)));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ValidInMinutes),
            SigningCredentials = new SigningCredentials(_rsaSecurityKey, SecurityAlgorithms.RsaSha256)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}