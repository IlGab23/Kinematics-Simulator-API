using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Application.Interfaces.Security;
using KinematicsSimulator.Domain.ResultPattern;
using MediatR;

namespace KinematicsSimulator.Application.Features.User.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<string>>;

public class LoginUserHandler(IUserRepository userRepo, IPasswordHasher passHasher, IJwtProvider jwtProvider) : IRequestHandler<LoginUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null) return Error.LoginWrongCredentials;

        if (!await passHasher.Verify(user.PasswordHash, request.Password)) return Error.LoginWrongCredentials;

        string accessToken = jwtProvider.GenerateToken(user);

        return accessToken;
    }
}