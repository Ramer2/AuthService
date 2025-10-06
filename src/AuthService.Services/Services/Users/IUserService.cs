using AuthService.DAL.Models;
using AuthService.Services.DTOs.Users;

namespace AuthService.Services.Services.Users;

public interface IUserService
{
    public Task<List<UserDto>>  GetAllUsersAsync(CancellationToken cancellationToken);
    
    public Task<UserDto> GetUserByIdAsync(GetUserByIdDto getUserByIdDto, CancellationToken cancellationToken);
    
    public Task<UserDto> GetUserByUsernameAsync(GetUserByUsernameDto getUserByUsernameDto,CancellationToken cancellationToken);
    
    public Task<UserDto> GetUserByEmailAsync(GetUserByEmailDto getUserByEmailDto, CancellationToken cancellationToken);
    
    public Task<UserDto> CreateUserCredentialsAsync(CreateUserDto createUserDto, CancellationToken cancellationToken);
}