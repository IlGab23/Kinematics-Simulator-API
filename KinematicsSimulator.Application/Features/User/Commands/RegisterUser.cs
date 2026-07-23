using KinematicsSimulator.Application.Interfaces;
using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Application.Interfaces.Security;
using KinematicsSimulator.Domain.Entities.ValueObjects;
using KinematicsSimulator.Domain.ResultPattern;
using MediatR;
using DomainUser = KinematicsSimulator.Domain.Entities.User;

namespace KinematicsSimulator.Application.Features.User.Commands;

public record RegisterUserCommand(string UserName, string Email, string Password) : IRequest<Result<Guid>>;

public class RegisterUserHandler(IApplicationDbContext appDbContext, IUserRepository userRepo, IPasswordHasher passHasher) : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await userRepo.GetByEmailAsync(request.Email, cancellationToken);
        if (userExist is not null) return Error.RegistrationEmailExist;

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure) return emailResult.errorList;

        string hashedPassword = await passHasher.Hash(request.Password);

        var userResult = DomainUser.Create(request.UserName, emailResult.Value, hashedPassword);
        if (userResult.IsFailure) return userResult.errorList;

        await userRepo.AddAsync(userResult.Value, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return userResult.Value.Id;
    }
}