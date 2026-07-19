using FluentAssertions;
using KinematicsSimulator.Domain.Services;

namespace Kinematics.Domain.UnitTests;

public class MruTest
{
    [Theory]
    [InlineData(10.0, 4.0, 16.0, 74.0)]
    [InlineData(0.0, 5.5, 10.0, 55.0)]
    [InlineData(-5.0, 2.0, 8.0, 11.0)]
    [InlineData(20.0, -3.0, 15.0, -25.0)]
    [InlineData(-15.5, -4.5, 20.0, -105.5)]
    public void MRU_EquationOfMotion_GoodValues_ShouldReturnCorrectValue(double initPos, double velocity, double time, double expectedResult)
    {
        var result = PhysicsEngine.MRU_EquationOfMotion(initPos, velocity, time);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(5.0, 10.0, 50.0)]
    [InlineData(-3.5, 4.0, -14.0)]
    [InlineData(10.0, 2.5, 25.0)]
    [InlineData(-2.0, 15.0, -30.0)]
    [InlineData(0.0, 5.0, 0.0)]
    public void MRU_Distance_GoodValues_ShouldReturnCorrectValue(double velocity, double time, double expectedResult)
    {
        var result = PhysicsEngine.MRU_Distance(velocity, time);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(100.0, 10.0, 10.0)]
    [InlineData(-50.0, 5.0, -10.0)]
    [InlineData(25.5, 2.0, 12.75)]
    [InlineData(-10.0, 4.0, -2.5)]
    [InlineData(0.0, 8.0, 0.0)]
    public void MRU_Velocity_GoodValues_ShouldReturnCorrectValue(double distance, double time, double expectedResult)
    {
        var result = PhysicsEngine.MRU_Velocity(distance, time);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(100.0, 20.0, 5.0)]
    [InlineData(-50.0, -10.0, 5.0)]
    [InlineData(40.0, 5.0, 8.0)]
    [InlineData(-20.0, 4.0, -5.0)]
    [InlineData(0.0, 10.0, 0.0)]
    public void MRU_Time_GoodValues_ShouldReturnCorrectValue(double distance, double velocity, double expectedResult)
    {
        var result = PhysicsEngine.MRU_Time(distance, velocity);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(100.0, 0.0)]
    [InlineData(100.0, -5.0)]
    public void MRU_Velocity_NegativeOrZeroTime_ShouldReturnFailure(double distance, double time)
    {
        var result = PhysicsEngine.MRU_Velocity(distance, time);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "PhysicsEngine.MRU_VelocityNegativeTimeValue");
    }

    [Theory]
    [InlineData(100.0, 0.0)]
    public void MRU_Time_ZeroVelocity_ShouldReturnFailure(double distance, double velocity)
    {
        var result = PhysicsEngine.MRU_Time(distance, velocity);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "PhysicsEngine.MRU_TimeZeroVelocityValue");
    }
}
