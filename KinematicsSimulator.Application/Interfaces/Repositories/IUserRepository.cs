using KinematicsSimulator.Domain.Entities;

namespace KinematicsSimulator.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(User user, CancellationToken cancellationToken = default);
}
