using FluentAssertions;
using KinematicsSimulator.Domain.Entities;
using KinematicsSimulator.Domain.Entities.ValueObjects;

namespace Kinematics.Domain.UnitTests.EntitiesTests;

public class KinematicSimulationTests
{
    [Fact]
    public void KinematicSimulation_Create_GoodValues_ShouldReturnSimulation()
    {
        var simTypeResult = SimulationType.Create("MRU");
        var resultValueResult = ResultValue.Create(10.5);
        var result = KinematicSimulation.Create(Guid.NewGuid(), simTypeResult.Value, resultValueResult.Value, DateTimeOffset.UtcNow);

        result.IsSuccess.Should().BeTrue();
        result.Value.SimulationType.Value.Should().Be("MRU");
    }

    [Fact]
    public void KinematicSimulation_Create_EmptyUserId_ShouldReturnErrorEmptyUserId()
    {
        var simTypeResult = SimulationType.Create("MRU");
        var resultValueResult = ResultValue.Create(10.5);
        var result = KinematicSimulation.Create(Guid.Empty, simTypeResult.Value, resultValueResult.Value, DateTimeOffset.UtcNow);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "KinematicSimulation.EmptyUserId");
    }

    [Fact]
    public void KinematicSimulation_Create_EmptyCreatedAt_ShouldReturnErrorEmptyCreatedAt()
    {
        var simTypeResult = SimulationType.Create("MRU");
        var resultValueResult = ResultValue.Create(10.5);
        var result = KinematicSimulation.Create(Guid.NewGuid(), simTypeResult.Value, resultValueResult.Value, default);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "KinematicSimulation.EmptyCreatedAt");
    }
}
