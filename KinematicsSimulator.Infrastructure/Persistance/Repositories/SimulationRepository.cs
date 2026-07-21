using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinematicsSimulator.Infrastructure.Persistance.Repositories;

public class SimulationRepository(ApplicationDbContext dbContext) : ISimulationRepository
{
    public async Task<bool> AddAsync(KinematicSimulation kinematicSimulation, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Simulations.AddAsync(kinematicSimulation, cancellationToken);
        return entry.State == Microsoft.EntityFrameworkCore.EntityState.Added;
    }

    public async Task<IReadOnlyList<KinematicSimulation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        #pragma warning disable CDT1003
        return await dbContext.Simulations.AsNoTracking().Where(ks => ks.UserId == userId).ToListAsync(cancellationToken);
    }

}
