using KinematicsSimulator.Domain.ResultPattern;

namespace KinematicsSimulator.Domain.Services;

public static class PhysicsEngine
{
    //MRU Calculations
    public static Result<double> MRU_EquationOfMotion(double initPos, double velocity, double timeInSeconds)
    {
        return initPos + (velocity * timeInSeconds);
    }
    public static Result<double> MRU_Distance(double velocity, double timeInSeconds)
    {
        return velocity * timeInSeconds;
    }
    public static Result<double> MRU_Velocity(double distance, double timeInSeconds)
    {
        if(timeInSeconds <= 0) return Error.Validation("PhysicsEngine.MRU_VelocityNegativeTimeValue", "Cannot assign 0 or less to time");
        return distance / timeInSeconds;
    }
    public static Result<double> MRU_Time(double distance, double velocity)
    {
        if(velocity == 0) return Error.Validation("PhysicsEngine.MRU_TimeZeroVelocityValue", "Cannot assign 0 to velocity");
        return distance / velocity;
    }

    //MRUA Calculations
    public static Result<double> MRUA_EquationOfMotion(double initPos, double initVelocity, double acceleration, double timeInSeconds)
    {
        return initPos + (initVelocity * timeInSeconds) + ((1.0 / 2.0) * acceleration * (timeInSeconds * timeInSeconds));
    }
    public static Result<double> MRUA_FinalVelocity(double initVelocity, double acceleration, double timeInSeconds)
    {
        return initVelocity + (acceleration * timeInSeconds);
    }
    public static Result<double> MRUA_Acceleration(double velocity, double initVelocity, double timeInSeconds)
    {
        if(timeInSeconds <= 0) return Error.Validation("PhysicsEngine.MRUA_AccelerationNegativeTimeValue", "Cannot assign 0 or less to time");
        return (velocity - initVelocity) / timeInSeconds;
    }
    public static Result<double> MRUA_Time(double velocity, double initVelocity, double acceleration)
    {
        if(acceleration == 0) return Error.Validation("PhysicsEngine.MRUA_TimeZeroAccelerationValue", "Cannot assign 0 to acceleration");
        return (velocity - initVelocity) / acceleration;
    }

}
