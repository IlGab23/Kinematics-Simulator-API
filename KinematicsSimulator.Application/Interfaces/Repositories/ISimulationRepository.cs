using KinematicsSimulator.Domain.Entities;

namespace KinematicsSimulator.Application.Interfaces.Repositories;

public interface ISimulationRepository
{
    Task<bool> AddAsync(KinematicSimulation kinematicSimulation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<KinematicSimulation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
