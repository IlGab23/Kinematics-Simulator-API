namespace KinematicsSimulator.Domain.Exceptions;

#pragma warning disable RCS1194
public class InvalidResultPatternException : DomainException
{
    public InvalidResultPatternException() : base(
        "Invalid Result state: a successful result cannot contain errors, and a failed result must contain at least one error.",
        500,
        "Result pattern value error")
    {
    }

    public InvalidResultPatternException(string message) : base(message, 500, "Result pattern value error")
    {
    }

}
