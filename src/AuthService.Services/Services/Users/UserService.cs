using AuthService.DAL.Context;
using AuthService.DAL.Models;
using AuthService.Services.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Services.Users;

public class UserService : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new();
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

    public async Task<User> GetUserByUsernameAsync(GetUserByUsernameDto getUserByUsernameDto, CancellationToken cancellationToken)
    {
        if (getUserByUsernameDto == null || getUserByUsernameDto.Username == null)
            throw new ArgumentException("Invalid user username.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == getUserByUsernameDto.Username, cancellationToken);

        if (user == null)
            throw new FileNotFoundException($"No user found with the username {getUserByUsernameDto.Username}.");
        
        return user;
    }

    public async Task<User> GetUserByEmailAsync(GetUserByEmailDto getUserByEmailDto, CancellationToken cancellationToken)
    {
        if (getUserByEmailDto == null || getUserByEmailDto.Email == null)
            throw new ArgumentException("Invalid user username.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == getUserByEmailDto.Email, cancellationToken);

        if (user == null)
            throw new FileNotFoundException($"No user found with the enail {getUserByEmailDto.Email}.");
        
        return user;
    }

    public async Task<User> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        if (createUserDto == null)
            throw new ArgumentException("No data was provided.");
        
        // everything else should be checked by the attributes in the dto (i hope at least)
        var newUser = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
        };

        newUser.HashedPassword = _passwordHasher.HashPassword(newUser, createUserDto.Password);
        
        await _context.Users.AddAsync(newUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return newUser;
    }
}