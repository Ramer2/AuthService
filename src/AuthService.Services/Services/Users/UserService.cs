using AuthService.DAL.Context;
using AuthService.DAL.Models;
using AuthService.Services.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Services.Users;

public class UserService : IUserService
{
    private readonly CredentialsDatabaseContext _context;

    public UserService(CredentialsDatabaseContext context)
    {
        _context = context;
    }


    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(GetUserByIdDto getUserByIdDto, CancellationToken cancellationToken)
    {
        if (getUserByIdDto == null || getUserByIdDto.UserId == null)
            throw new ArgumentException("Invalid user id.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == getUserByIdDto.UserId, cancellationToken);

        if (user == null)
            throw new FileNotFoundException($"No user found with the id {getUserByIdDto.UserId}.");
        
        return user;
    }

    public Task<User> GetUserByUsernameAsync(GetUserByUsernameDto getUserByUsernameDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByEmailAsync(GetUserByEmailDto getUserByEmailDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}