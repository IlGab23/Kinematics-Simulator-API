using FluentAssertions;
using KinematicsSimulator.Application.Features.User.Commands;
using KinematicsSimulator.Application.Interfaces;
using KinematicsSimulator.Application.Interfaces.Repositories;
using KinematicsSimulator.Application.Interfaces.Security;
using KinematicsSimulator.Domain.Entities.ValueObjects;
using Moq;
using Xunit.Sdk;
using DomainUser = KinematicsSimulator.Domain.Entities.User;

namespace Kinematics.Application.UnitTests;

public class RegistrationTests
{
    [Fact]
    public async Task RegisterUser_GoodRegistration_ShouldReturnUserId()
    {
        const string email = "testEmail@gmail.com";
        const string userName = "testUserName";
        const string password = "testPassword";

        var command = new RegisterUserCommand(userName, email, password);

        var fakeUserRepo = new Mock<IUserRepository>();
        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakePasswordHasher = new Mock<IPasswordHasher>();

        fakeUserRepo
            .Setup(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DomainUser?)null);

        var handler = new RegisterUserHandler(fakeAppDbContext.Object, fakeUserRepo.Object, fakePasswordHasher.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        fakeUserRepo.Verify(repo => repo.AddAsync(It.IsAny<DomainUser>(), It.IsAny<CancellationToken>()), Times.Once);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_EmailArleadyExistOnDb_ShouldReturnErrorOnEmailExistVerification()
    {
        const string email = "testEmail@gmail.com";
        const string userName = "testUserName";
        const string password = "testPassword";

        var command = new RegisterUserCommand(userName, email, password);

        var existingEmail = Email.Create("testExistingEmail@gmail.com").Value;
        var existingUser = DomainUser.Create("AlreadyExistUser", existingEmail, "APasswordForAnUserThatAlreadyExist").Value;

        var fakeUserRepo = new Mock<IUserRepository>();
        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakePasswordHasher = new Mock<IPasswordHasher>();

        fakeUserRepo
            .Setup(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var handler = new RegisterUserHandler(fakeAppDbContext.Object, fakeUserRepo.Object, fakePasswordHasher.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "User.DuplicateEmail");

        fakeUserRepo.Verify(repo => repo.AddAsync(It.IsAny<DomainUser>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUser_BadEmail_ShouldReturnErrorOnEmailCreation()
    {
        const string email = "testEmailgmail.com";
        const string userName = "testUserName";
        const string password = "testPassword";

        var command = new RegisterUserCommand(userName, email, password);

        var fakeUserRepo = new Mock<IUserRepository>();
        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakePasswordHasher = new Mock<IPasswordHasher>();

        fakeUserRepo
            .Setup(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DomainUser?)null);

        var handler = new RegisterUserHandler(fakeAppDbContext.Object, fakeUserRepo.Object, fakePasswordHasher.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().Contain(e => e.Name == "Email.InvalidEmailFormat");

        fakeUserRepo.Verify(repo => repo.AddAsync(It.IsAny<DomainUser>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUser_CreationUserFailed_ShouldReturnErrorOnEmailCreation()
    {
        const string email = "testEmail@gmail.com";
        const string userName = "te";
        const string password = "testPassword";

        var command = new RegisterUserCommand(userName, email, password);

        var fakeUserRepo = new Mock<IUserRepository>();
        var fakeAppDbContext = new Mock<IApplicationDbContext>();
        var fakePasswordHasher = new Mock<IPasswordHasher>();

        fakeUserRepo
            .Setup(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DomainUser?)null);

        var handler = new RegisterUserHandler(fakeAppDbContext.Object, fakeUserRepo.Object, fakePasswordHasher.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().Contain(e => e.Name == "User.InvalidUserNameLength");

        fakeUserRepo.Verify(repo => repo.AddAsync(It.IsAny<DomainUser>(), It.IsAny<CancellationToken>()), Times.Never);
        fakeAppDbContext.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

}
