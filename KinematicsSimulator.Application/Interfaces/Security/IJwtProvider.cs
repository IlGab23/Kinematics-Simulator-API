using KinematicsSimulator.Domain.Entities;

namespace KinematicsSimulator.Application.Interfaces.Security;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
