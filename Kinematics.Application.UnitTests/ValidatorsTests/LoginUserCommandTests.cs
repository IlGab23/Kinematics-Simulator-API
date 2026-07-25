using FluentAssertions;
using KinematicsSimulator.Application.Features.User.Commands;
using KinematicsSimulator.Application.Validators.LoginValidators;

namespace Kinematics.Application.UnitTests.ValidatorsTests;

public class LoginUserCommandTests
{
    [Fact]
    public async Task Validator_GoodValues_ShouldBeOk()
    {
        const string email = "UserName@gmail.com";
        const string password = "superPassword!123";

        var validator = new LoginUserCommandValidator();
        var command = new LoginUserCommand(email, password);

        var result = await validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_BadValues_ShouldReturnErrors()
    {
        const string email = "UserNamegmail.com";
        const string password = "";

        var validator = new LoginUserCommandValidator();
        var command = new LoginUserCommand(email, password);

        var result = await validator.ValidateAsync(command);

        result.Errors.Should().HaveCount(3, "password Validator give 2 errors: 1 for empty and 1 for be lower than 6 char");
        result.IsValid.Should().BeFalse();
    }

    
}
