using System.Security.Claims;
using KinematicsSimulator.Application.Features.Simulations.Commands;
using KinematicsSimulator.Application.Features.Simulations.Queries;
using KinematicsSimulator.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KinematicsSimulator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController(ISender sender) : ControllerBase
{
    public record CreateSimulationRequest(string SimType, double? S, double? T, double? V, double? A, double? InitPos, double? InitVelocity, string TargetVariable);

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateSimulation([FromBody] CreateSimulationRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized("User ID is missing or invalid in the token.");

        var command = new CreateSimulationCommand(
            userId.Value,
            request.SimType,
            request.S,
            request.T,
            request.V,
            request.A,
            request.InitPos,
            request.InitVelocity,
            request.TargetVariable
        );

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.errorList);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetSimulations(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized("User ID is missing or invalid in the token.");

        var query = new GetSimulationsQuery(userId.Value);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.errorList);
    }

    private Guid? GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (Guid.TryParse(userIdString, out Guid userId))
        {
            return userId;
        }

        return null;
    }
}
