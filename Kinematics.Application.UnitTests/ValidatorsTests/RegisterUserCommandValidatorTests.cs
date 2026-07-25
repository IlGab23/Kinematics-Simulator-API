using FluentAssertions;
using KinematicsSimulator.Application.Features.User.Commands;
using KinematicsSimulator.Application.Validators.RegistrationValidators;

namespace Kinematics.Application.UnitTests.ValidatorsTests;

public class RegisterUserCommandValidatorTests
{
    [Fact]
    public async Task Validator_GoodValues_ShouldBeOk()
    {
        const string userName = "UserName";
        const string email = "UserName@gmail.com";
        const string password = "superPassword!123";

        var validator = new RegisterUserCommandValidator();
        var command = new RegisterUserCommand(userName, email, password);

        var result = await validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_BadValues_ShouldReturnErrors()
    {
        const string userName = "Us";
        const string email = "UserNamegmail.com";
        const string password = "superPassword123";

        var validator = new RegisterUserCommandValidator();
        var command = new RegisterUserCommand(userName, email, password);

        var result = await validator.ValidateAsync(command);

        result.Errors.Should().HaveCount(3);
        result.IsValid.Should().BeFalse();
    }

    
}
