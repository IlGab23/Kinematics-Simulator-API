namespace KinematicsSimulator.Domain.Exceptions;

public class InvalidResultPatternException : Exception
{
    public InvalidResultPatternException() : base("Invalid Result state: a successful result cannot contain errors, and a failed result must contain at least one error.") {}

    public InvalidResultPatternException(string? message) : base(message)
    {
    }

    public InvalidResultPatternException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
