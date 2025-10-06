using System.Data;
using AuthService.DAL.Context;
using AuthService.DAL.Models;
using AuthService.Services.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
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

    public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var result = new List<UserDto>();
        var users =  await _context.Users
            .Include(u => u.Roles)
            .Include(u => u.Permissions)
            .ToListAsync(cancellationToken);
        
        foreach (var user in users)
        {
            result.Add(new UserDto
            {
                UserId = user.UserId.ToString(),
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt.ToString(),
                Roles = user.Roles.Select(r => r.RoleName).ToList(),
                Permissions = user.Permissions.Select(p => p.PermissionName).ToList(),
            });
        }
        
        return result;
    }

    public async Task<UserDto> GetUserByIdAsync(GetUserByIdDto getUserByIdDto, CancellationToken cancellationToken)
    {
        if (getUserByIdDto == null || getUserByIdDto.UserId == null)
            throw new ArgumentException("Invalid user id.");

        var user = await _context.Users
            .Include(u => u.Roles)
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.UserId.ToString() == getUserByIdDto.UserId, cancellationToken);

        if (user == null)
            throw new FileNotFoundException($"No user found with the id {getUserByIdDto.UserId}.");
        
        return new UserDto
        {
            UserId = user.UserId.ToString(),
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt.ToString(),
            Roles = user.Roles.Select(r => r.RoleName).ToList(),
            Permissions = user.Permissions.Select(p => p.PermissionName).ToList(),
        };
    }

    public async Task<UserDto> GetUserByUsernameAsync(GetUserByUsernameDto getUserByUsernameDto, CancellationToken cancellationToken)
    {
        if (getUserByUsernameDto == null || getUserByUsernameDto.Username == null)
            throw new ArgumentException("Invalid user username.");

        var user = await _context.Users
            .Include(u => u.Roles)
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Username == getUserByUsernameDto.Username, cancellationToken);

        if (user == null)
            throw new FileNotFoundException($"No user found with the username {getUserByUsernameDto.Username}.");
        
        return new UserDto
        {
            UserId = user.UserId.ToString(),
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt.ToString(),
            Roles = user.Roles.Select(r => r.RoleName).ToList(),
            Permissions = user.Permissions.Select(p => p.PermissionName).ToList(),
        };
    }

    public async Task<UserDto> GetUserByEmailAsync(GetUserByEmailDto getUserByEmailDto, CancellationToken cancellationToken)
    {
        if (getUserByEmailDto == null || getUserByEmailDto.Email == null)
            throw new ArgumentException("Invalid user username.");

        var user = await _context.Users
            .Include(u => u.Roles)
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Email == getUserByEmailDto.Email, cancellationToken);

        if (user == null)
            throw new FileNotFoundException($"No user found with the enail {getUserByEmailDto.Email}.");
        
        return new UserDto
        {
            UserId = user.UserId.ToString(),
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt.ToString(),
            Roles = user.Roles.Select(r => r.RoleName).ToList(),
            Permissions = user.Permissions.Select(p => p.PermissionName).ToList(),
        };
    }

    public async Task<UserDto> CreateUserCredentialsAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        if (createUserDto == null)
            throw new ArgumentException("No data was provided.");
        
        var emailUsernameCheck = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == createUserDto.Email || u.Username == createUserDto.Username, cancellationToken);
        
        if (emailUsernameCheck != null)
            throw new DuplicateNameException("User with this data already exists.");
        
        // everything else should be checked by the attributes in the dto (i hope at least)

        var roles = new List<Role>();
        if (createUserDto.Roles.Count != 0)
        {
            foreach (var roleName in createUserDto.Roles)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName ==  roleName, cancellationToken);
                if (role == null)
                    throw new FileNotFoundException($"No role found with the name {roleName}.");
                
                roles.Add(role);
            }
        }

        var permissions = new List<Permission>();
        if (!(createUserDto.Permissions == null || createUserDto.Permissions.Count == 0))
        {
            foreach (var permissionName in createUserDto.Permissions)
            {
                var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionName == permissionName, cancellationToken);
                if (permission == null)
                    throw new FileNotFoundException($"No permission found with the name {permissionName}.");
                
                permissions.Add(permission);
            }            
        }

        var newUser = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            CreatedAt = DateTime.Now,
            IsActive = true,
            Permissions = permissions,
            Roles = roles
        };

        newUser.HashedPassword = _passwordHasher.HashPassword(newUser, createUserDto.Password);
        
        await _context.Users.AddAsync(newUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == createUserDto.Username, cancellationToken);
        
        var newUserDto = new UserDto
        {
            UserId = user.UserId.ToString(),
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt.ToString(),
            IsActive = user.IsActive.Value,
            Roles = user.Roles.Select(r => r.RoleName).ToList(),
            Permissions = user.Permissions.Select(p => p.PermissionName).ToList()
        };
        
        return newUserDto;
    }
}