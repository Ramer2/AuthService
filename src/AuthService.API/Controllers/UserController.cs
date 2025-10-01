using AuthService.Services.DTOs.Users;
using AuthService.Services.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [Route("api/users")]
    public async Task<IResult> GetAllUsers(CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await _userService.GetAllUsersAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("api/users/by-id")]
    public async Task<IResult> GetUserById([FromBody] GetUserByIdDto getUserByIdDto, CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await _userService.GetUserByIdAsync(getUserByIdDto, cancellationToken));
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
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

    [HttpPost]
    [Route("api/users/by-username")]
    public async Task<IResult> GetUserByUsername([FromBody] GetUserByUsernameDto getUserByUsernameDto, CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await _userService.GetUserByUsernameAsync(getUserByUsernameDto, cancellationToken));
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
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

    [HttpPost]
    [Route("api/users/by-email")]
    public async Task<IResult> GetUserByEmail([FromBody] GetUserByEmailDto getUserByEmailDto, CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await _userService.GetUserByEmailAsync(getUserByEmailDto, cancellationToken));
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
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