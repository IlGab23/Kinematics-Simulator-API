namespace KinematicsSimulator.Domain.Exceptions;

public class InvalidCorsRouteException : DomainException
{

    public InvalidCorsRouteException() : base("One or more allowed front-end CORS routes are missing from the configuration.")
    {
        StatusCode = 500;
        Title = "Cors configuration missing";
    }

    public InvalidCorsRouteException(string? message) : base(message)
    {
        StatusCode = 500;
        Title = "Cors configuration missing";
    }

    public InvalidCorsRouteException(string? message, Exception? innerException) : base(message, innerException)
    {
        StatusCode = 500;
        Title = "Cors configuration missing";
    }

}
