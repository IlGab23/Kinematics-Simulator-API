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

    public async Task<(IReadOnlyList<KinematicSimulation>, int)> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        int totalDataRows = await dbContext.Simulations.Where(kd => kd.UserId == userId).CountAsync(cancellationToken);
        
        #pragma warning disable CDT1003
        IReadOnlyList<KinematicSimulation> dataList = await dbContext.Simulations.AsNoTracking()
            .Where(ks => ks.UserId == userId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

            return (dataList, totalDataRows);
    }

}
