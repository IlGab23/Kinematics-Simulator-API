using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using KinematicsSimulator.Domain.ResultPattern;
using MediatR;

namespace KinematicsSimulator.Application.Features.Simulations.Queries;

public record GetSimulationsQuery(Guid UserId) : IRequest<Result<SimulationsOutput>>;
public record SimulationsOutput(IReadOnlyList<SimulationDTO> Simulations);
public record SimulationDTO(Guid SimId, Guid UserId, string SimType, double ResultValue, DateTimeOffset CreatedAt);

public class GetSimulationsHandler(ISimulationRepository simRepo) : IRequestHandler<GetSimulationsQuery, Result<SimulationsOutput>>
{
    public async Task<Result<SimulationsOutput>> Handle(GetSimulationsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<KinematicSimulation> simulations = await simRepo.GetByUserIdAsync(request.UserId, cancellationToken);

        List<SimulationDTO> outPutSimulations = new();

        foreach (var sim in simulations)
        {
            outPutSimulations.Add(new SimulationDTO(sim.Id, sim.UserId, sim.SimulationType.Value, sim.ResultValue.Value, sim.CreatedAt));
        }

        return new SimulationsOutput(outPutSimulations);
    }
}
