using FluentAssertions;
using KinematicsSimulator.Domain.Services;

namespace Kinematics.Domain.UnitTests;

public class MruaTest
{
    [Theory]
    [InlineData(0.0, 0.0, 9.8, 2.0, 19.6)]
    [InlineData(10.0, 5.0, -2.0, 4.0, 14.0)]
    [InlineData(-5.0, 0.0, 3.5, 10.0, 170.0)]
    [InlineData(0.0, -10.0, -9.8, 3.0, -74.1)]
    [InlineData(20.0, -5.0, 1.0, 5.0, 7.5)]
    public void MRUA_EquationOfMotion_GoodValues_ShouldReturnCorrectValue(double initPos, double initVelocity, double acceleration, double time, double expectedResult)
    {
        var result = PhysicsEngine.MRUA_EquationOfMotion(initPos, initVelocity, acceleration, time);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(0.0, 9.8, 5.0, 49.0)]
    [InlineData(10.0, -2.0, 3.0, 4.0)]
    [InlineData(-5.0, 1.5, 10.0, 10.0)]
    [InlineData(20.0, -9.8, 2.0, 0.4)]
    [InlineData(-10.0, -5.0, 4.0, -30.0)]
    public void MRUA_FinalVelocity_GoodValues_ShouldReturnCorrectValue(double initVelocity, double acceleration, double time, double expectedResult)
    {
        var result = PhysicsEngine.MRUA_FinalVelocity(initVelocity, acceleration, time);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(20.0, 0.0, 5.0, 4.0)]
    [InlineData(10.0, 20.0, 2.0, -5.0)]
    [InlineData(-15.0, -5.0, 5.0, -2.0)]
    [InlineData(0.0, 30.0, 3.0, -10.0)]
    [InlineData(-20.0, 10.0, 6.0, -5.0)]
    public void MRUA_Acceleration_GoodValues_ShouldReturnCorrectValue(double velocity, double initVelocity, double time, double expectedResult)
    {
        var result = PhysicsEngine.MRUA_Acceleration(velocity, initVelocity, time);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(20.0, 0.0, 4.0, 5.0)]
    [InlineData(0.0, 20.0, -5.0, 4.0)]
    [InlineData(-10.0, 10.0, -2.0, 10.0)]
    [InlineData(-30.0, -10.0, -4.0, 5.0)]
    [InlineData(50.0, 10.0, 8.0, 5.0)]
    public void MRUA_Time_GoodValues_ShouldReturnCorrectValue(double velocity, double initVelocity, double acceleration, double expectedResult)
    {
        var result = PhysicsEngine.MRUA_Time(velocity, initVelocity, acceleration);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeApproximately(expectedResult, 0.001);
    }

    [Theory]
    [InlineData(20.0, 0.0, 0.0)]
    [InlineData(10.0, 20.0, -2.0)]
    public void MRUA_Acceleration_NegativeOrZeroTime_ShouldReturnFailure(double velocity, double initVelocity, double time)
    {
        var result = PhysicsEngine.MRUA_Acceleration(velocity, initVelocity, time);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "PhysicsEngine.MRUA_AccelerationNegativeTimeValue");
    }

    [Theory]
    [InlineData(20.0, 0.0, 0.0)]
    public void MRUA_Time_ZeroAcceleration_ShouldReturnFailure(double velocity, double initVelocity, double acceleration)
    {
        var result = PhysicsEngine.MRUA_Time(velocity, initVelocity, acceleration);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "PhysicsEngine.MRUA_TimeZeroAccelerationValue");
    }
}
