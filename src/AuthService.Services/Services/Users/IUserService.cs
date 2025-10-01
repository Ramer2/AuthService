using AuthService.DAL.Models;
using AuthService.Services.DTOs.Users;

namespace AuthService.Services.Services.Users;

public interface IUserService
{
    public Task<List<User>>  GetAllUsersAsync(CancellationToken cancellationToken);
    
    public Task<User> GetUserByIdAsync(GetUserByIdDto getUserByIdDto, CancellationToken cancellationToken);
    
    public Task<User> GetUserByUsernameAsync(GetUserByUsernameDto getUserByUsernameDto,CancellationToken cancellationToken);
    
    public Task<User> GetUserByEmailAsync(GetUserByEmailDto getUserByEmailDto, CancellationToken cancellationToken);
    
    public Task<User> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken);
}