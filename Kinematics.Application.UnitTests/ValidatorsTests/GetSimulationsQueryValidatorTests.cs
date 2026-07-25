using FluentAssertions;
using KinematicsSimulator.Application.Features.Simulations.Queries;
using KinematicsSimulator.Application.Validators.SimulationValidators;

namespace Kinematics.Application.UnitTests.ValidatorsTests;

public class GetSimulationsQueryValidatorTests
{
    [Fact]
    public async Task Validator_GoodValues_ShouldBeOk()
    {
        var userId = Guid.NewGuid();

        var validator = new GetSimulationsQueryValidator();
        var query = new GetSimulationsQuery(userId);

        var result = await validator.ValidateAsync(query);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_BadValues_ShouldReturnErrors()
    {
        var userId = Guid.Empty;

        var validator = new GetSimulationsQueryValidator();
        var query = new GetSimulationsQuery(userId);

        var result = await validator.ValidateAsync(query);

        result.Errors.Should().HaveCount(1);
        result.IsValid.Should().BeFalse();
    }
}
