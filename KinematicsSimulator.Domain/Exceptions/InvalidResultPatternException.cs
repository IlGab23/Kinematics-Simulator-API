namespace KinematicsSimulator.Domain.Exceptions;

public class InvalidResultPatternException : DomainException
{
    public InvalidResultPatternException() : base("Invalid Result state: a successful result cannot contain errors, and a failed result must contain at least one error.")
    {
        StatusCode = 500;
        Title = "Result pattern value error";
    }

    public InvalidResultPatternException(string? message) : base(message)
    {
        StatusCode = 500;
        Title = "Result pattern value error";
    }

    public InvalidResultPatternException(string? message, Exception? innerException) : base(message, innerException)
    {
        StatusCode = 500;
        Title = "Result pattern value error";
    }

}
