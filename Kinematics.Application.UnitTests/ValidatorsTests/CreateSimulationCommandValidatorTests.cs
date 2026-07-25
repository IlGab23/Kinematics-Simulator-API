using FluentAssertions;
using KinematicsSimulator.Application.Features.Simulations.Commands;
using KinematicsSimulator.Application.Validators.SimulationValidators;

namespace Kinematics.Application.UnitTests.ValidatorsTests;

public class CreateSimulationCommandValidatorTests
{
    [Fact]
    public async Task Validator_GoodValues_ShouldBeOk()
    {
        var userId = Guid.NewGuid();
        const string simType = "MRUA";
        double? s = 100;
        double? t = 10;
        double? v = null;
        double? a = null;
        double? initPos = 0;
        double? initVelocity = null;
        const string targetVariable = "V";

        var validator = new CreateSimulationCommandValidator();
        var command = new CreateSimulationCommand(userId, simType, s, t, v, a, initPos, initVelocity, targetVariable);

        var result = await validator.ValidateAsync(command);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_BadValues_ShouldReturnErrors()
    {
        var userId = Guid.NewGuid();
        const string simType = "MRUAA";
        double? s = 100;
        double? t = 0;
        double? v = null;
        double? a = null;
        double? initPos = 0;
        double? initVelocity = null;
        const string targetVariable = "";

        var validator = new CreateSimulationCommandValidator();
        var command = new CreateSimulationCommand(userId, simType, s, t, v, a, initPos, initVelocity, targetVariable);

        var result = await validator.ValidateAsync(command);

        result.Errors.Should().HaveCount(3);
        result.IsValid.Should().BeFalse();
    }

    

}
