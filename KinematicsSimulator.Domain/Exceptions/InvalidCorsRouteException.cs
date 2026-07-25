namespace KinematicsSimulator.Domain.Exceptions;

public class InvalidCorsRouteException : Exception
{
    public InvalidCorsRouteException() : base("One or more allowed front-end CORS routes are missing from the configuration.")
    {
    }

    public InvalidCorsRouteException(string? message) : base(message)
    {
    }

    public InvalidCorsRouteException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
