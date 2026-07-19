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
        return distance / timeInSeconds;
    }
    public static Result<double> MRU_Time(double distance, double velocity)
    {
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
        return (velocity - initVelocity) / timeInSeconds;
    }
    public static Result<double> MRUA_Time(double velocity, double initVelocity, double acceleration)
    {
        return (velocity - initVelocity) / acceleration;
    }

}
