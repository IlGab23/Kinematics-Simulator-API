namespace KinematicsSimulator.Domain.Exceptions;

public class DomainException : Exception
{
    public int StatusCode { get; }
    public string Title { get; }

    public DomainException(string message, int statusCode, string title) : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }

}
