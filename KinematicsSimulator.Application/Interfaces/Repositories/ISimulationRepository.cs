using KinematicsSimulator.Domain.Entities;

namespace KinematicsSimulator.Application.Interfaces.Repositories;

public interface ISimulationRepository
{
    Task<bool> AddAsync(KinematicSimulation kinematicSimulation, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<KinematicSimulation>, int)> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
