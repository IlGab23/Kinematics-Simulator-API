using FluentAssertions;
using KinematicsSimulator.Domain.Entities.ValueObjects;

namespace Kinematics.Domain.UnitTests.EntitiesTests;

public class ResultValueTests
{
    [Fact]
    public void ResultValue_Create_GoodValue_ShouldReturnResultValue()
    {
        var result = ResultValue.Create(10.5);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(10.5);
    }

    [Fact]
    public void ResultValue_Create_NaN_ShouldReturnErrorIsNaN()
    {
        var result = ResultValue.Create(double.NaN);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "ResultValue.IsNaN");
    }

    [Fact]
    public void ResultValue_Create_Infinity_ShouldReturnErrorIsInfinity()
    {
        var result = ResultValue.Create(double.PositiveInfinity);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "ResultValue.IsInfinity");
    }

    [Fact]
    public void ResultValue_Create_NegativeInfinity_ShouldReturnErrorIsInfinity()
    {
        var result = ResultValue.Create(double.NegativeInfinity);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "ResultValue.IsInfinity");
    }
}
