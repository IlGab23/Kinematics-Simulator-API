using KinematicsSimulator.Application.Common.Models;
using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using KinematicsSimulator.Domain.ResultPattern;
using MediatR;

namespace KinematicsSimulator.Application.Features.Simulations.Queries;

public record GetSimulationsQuery(Guid UserId, int PageNumber, int PageSize) : IRequest<Result<PagedList<SimulationDTO>>>;
public record SimulationDTO(Guid SimId, Guid UserId, string SimType, double ResultValue, DateTimeOffset CreatedAt);

public class GetSimulationsHandler(ISimulationRepository simRepo) : IRequestHandler<GetSimulationsQuery, Result<PagedList<SimulationDTO>>>
{
    public async Task<Result<PagedList<SimulationDTO>>> Handle(GetSimulationsQuery request, CancellationToken cancellationToken)
    {
        (IReadOnlyList<KinematicSimulation> simL, int listCount) dbResult = await simRepo.GetByUserIdAsync(request.UserId,
                                                                            request.PageNumber,
                                                                            request.PageSize,
                                                                            cancellationToken);
        int totalPages = (int)Math.Ceiling((double)dbResult.listCount / (double)request.PageSize);

        List<SimulationDTO> outPutSimulations = new();

        foreach (var sim in dbResult.simL)
        {
            outPutSimulations.Add(new SimulationDTO(sim.Id, sim.UserId, sim.SimulationType.Value, sim.ResultValue.Value, sim.CreatedAt));
        }

        var pagedOutPut = new PagedList<SimulationDTO>(
            outPutSimulations,
            dbResult.listCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );

        return pagedOutPut;
    }
}
