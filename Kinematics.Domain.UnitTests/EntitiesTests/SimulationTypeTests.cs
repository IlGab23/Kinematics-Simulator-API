using FluentAssertions;
using KinematicsSimulator.Domain.Entities.ValueObjects;

namespace Kinematics.Domain.UnitTests.EntitiesTests;

public class SimulationTypeTests
{
    [Fact]
    public void SimulationType_Create_GoodValues_ShouldReturnSimulationTypeMRU()
    {
        var result = SimulationType.Create("MRU");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("MRU");
    }
    
    [Fact]
    public void SimulationType_Create_GoodValues_ShouldReturnSimulationTypeMRUA()
    {
        var result = SimulationType.Create("MRUA");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("MRUA");
    }

    [Fact]
    public void SimulationType_Create_EmptyValue_ShouldReturnErrorEmpty()
    {
        var result = SimulationType.Create(" ");

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "SimulationType.Empty");
    }

    [Fact]
    public void SimulationType_Create_WrongValue_ShouldReturnErrorWrongType()
    {
        var result = SimulationType.Create("INVALID");

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "SimulationType.WrongType");
    }

    [Fact]
    public void SimulationType_Create_LowerCaseAndWhiteSpace_ShouldReturnSimulationTypeMRU()
    {
        var result = SimulationType.Create("  mru  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("MRU");
    }
}
