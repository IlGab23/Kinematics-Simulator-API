using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinematicsSimulator.Infrastructure.Persistance.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Users.AddAsync(user, cancellationToken);
        return entry.State == Microsoft.EntityFrameworkCore.EntityState.Added;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken: cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

}
