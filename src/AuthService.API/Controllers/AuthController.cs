using AuthService.DAL.Context;
using AuthService.DAL.Models;
using AuthService.Services.DTOs.Auth;
using AuthService.Services.Services.Tokens;
using AuthService.Services.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly CredentialsDatabaseContext _context;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthController(CredentialsDatabaseContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("/api/login")]
    public async Task<IResult> Auth(LoginUserDto loginUserDto, CancellationToken cancellationToken)
    {
        try
        {
            var foundUser = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Email == loginUserDto.Login || u.Username == loginUserDto.Login,
                    cancellationToken);

            if (foundUser == null)
                throw new FileNotFoundException("No user found with the given credentials.");

            var verificationResult =
                _passwordHasher.VerifyHashedPassword(foundUser, foundUser.HashedPassword, loginUserDto.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
                return Results.Unauthorized();

            var token = new TokenDto
            {
                AccessToken = _tokenService.GenerateToken(
                    foundUser.Username,
                    foundUser.Email,
                    foundUser.Roles.ToList(),
                    foundUser.Permissions.ToList()
                )
            };

            return Results.Ok(token);
        }
        catch (FileNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}