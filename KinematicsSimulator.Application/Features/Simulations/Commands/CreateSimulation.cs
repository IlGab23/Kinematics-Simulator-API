using KinematicsSimulator.Application.Interfaces;
using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using KinematicsSimulator.Domain.Entities.ValueObjects;
using KinematicsSimulator.Domain.ResultPattern;
using KinematicsSimulator.Domain.Services;
using MediatR;

namespace KinematicsSimulator.Application.Features.Simulations.Commands;

public record CreateSimulationCommand(Guid UserId,
string SimType,
double? S,
double? T,
double? V,
double? A,
double? InitPos,
double? InitVelocity,
string TargetVariable) : IRequest<Result<SimOutput>>;
public record SimOutput(Guid Id, double Result);

public class CreateSimulationHandler(IApplicationDbContext appDbContext, ISimulationRepository simRepo) : IRequestHandler<CreateSimulationCommand, Result<SimOutput>>
{
    public async Task<Result<SimOutput>> Handle(CreateSimulationCommand request, CancellationToken cancellationToken)
    {
        var simTypeResult = SimulationType.Create(request.SimType);
        if (simTypeResult.IsFailure) return simTypeResult.errorList;

        if(string.IsNullOrWhiteSpace(request.TargetVariable))
            return Error.Validation("CreateSimulation.MissingTargetVariable", "Target variable is required.");

        Result<double> simResult = (simTypeResult.Value.Value, request.TargetVariable.ToUpper()) switch
        {
            ("MRU", "S") when request.V.HasValue && request.T.HasValue => PhysicsEngine.MRU_EquationOfMotion(request.InitPos ?? 0, request.V.Value, request.T.Value),
            ("MRU", "V") when request.S.HasValue && request.T.HasValue => PhysicsEngine.MRU_Velocity(request.S.Value, request.T.Value),
            ("MRU", "T") when request.S.HasValue && request.V.HasValue => PhysicsEngine.MRU_Time(request.S.Value, request.V.Value),

            ("MRUA", "S") when request.InitVelocity.HasValue && request.A.HasValue && request.T.HasValue => PhysicsEngine.MRUA_EquationOfMotion(request.InitPos ?? 0, request.InitVelocity.Value, request.A.Value, request.T.Value),
            ("MRUA", "V") when request.InitVelocity.HasValue && request.A.HasValue && request.T.HasValue => PhysicsEngine.MRUA_FinalVelocity(request.InitVelocity.Value, request.A.Value, request.T.Value),
            ("MRUA", "A") when request.V.HasValue && request.InitVelocity.HasValue && request.T.HasValue => PhysicsEngine.MRUA_Acceleration(request.V.Value, request.InitVelocity.Value, request.T.Value),
            ("MRUA", "T") when request.V.HasValue && request.InitVelocity.HasValue && request.A.HasValue => PhysicsEngine.MRUA_Time(request.V.Value, request.InitVelocity.Value, request.A.Value),

            _ => Error.Validation("CreateSimulation.InvalidParameters", "Invalid parameters for the selected simulation type and target variable.")
        };

        if (simResult.IsFailure) return simResult.errorList;

        var resultValueResult = ResultValue.Create(simResult.Value);
        if (resultValueResult.IsFailure) return resultValueResult.errorList;

        var simulationResult = KinematicSimulation.Create(
            request.UserId,
            simTypeResult.Value,
            resultValueResult.Value,
            DateTimeOffset.UtcNow);

        if (simulationResult.IsFailure) return simulationResult.errorList;

        var simulation = simulationResult.Value;

        await simRepo.AddAsync(simulation, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new SimOutput(simulation.Id, simulation.ResultValue.Value);
    }
}
