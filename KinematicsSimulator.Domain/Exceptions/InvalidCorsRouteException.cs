namespace KinematicsSimulator.Domain.Exceptions;

#pragma warning disable RCS1194
public class InvalidCorsRouteException : DomainException
{

    public InvalidCorsRouteException() : base(
        "One or more allowed front-end CORS routes are missing from the configuration.",
        500,
        "Cors configuration missing")
    {
    }

    public InvalidCorsRouteException(string message) : base(message, 500, "Cors configuration missing")
    {
    }

}
