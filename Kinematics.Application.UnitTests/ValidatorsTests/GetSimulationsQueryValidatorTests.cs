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
        int pageNumber = 1;
        int pageSize = 10;

        var validator = new GetSimulationsQueryValidator();
        var query = new GetSimulationsQuery(userId, pageNumber, pageSize);

        var result = await validator.ValidateAsync(query);

        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_BadValues_ShouldReturnErrors()
    {
        var userId = Guid.Empty;
        int pageNumber = 0;
        int pageSize = 100;

        var validator = new GetSimulationsQueryValidator();
        var query = new GetSimulationsQuery(userId, pageNumber, pageSize);

        var result = await validator.ValidateAsync(query);

        result.Errors.Should().NotBeEmpty();
        result.IsValid.Should().BeFalse();
    }
}
