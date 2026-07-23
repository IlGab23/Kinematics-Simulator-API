using KinematicsSimulator.Application.Features.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KinematicsSimulator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ISender sender) : ControllerBase
{
    public record RegisterUserRequest(string UserName, string Email, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.UserName, request.Email, request.Password);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.errorList);
    }

    public record LoginUserRequest(string Email, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.errorList);
    }
}
