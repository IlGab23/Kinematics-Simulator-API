using KinematicsSimulator.Domain.Entities.ValueObjects;
using KinematicsSimulator.Domain.ResultPattern;

namespace KinematicsSimulator.Domain.Entities;
#pragma warning disable RCS1170, IDE0028

public sealed class User
{
    private readonly List<KinematicSimulation> _kinematicSimulations = new();

    public Guid Id { get; init; }
    public string UserName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }

    public IReadOnlyCollection<KinematicSimulation> KinematicSimulations => _kinematicSimulations.AsReadOnly();

    private User(Guid id, string userName, Email email, string passwordHash)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }

    #pragma warning disable CS8618
    private User() { }

    public static Result<User> Create(string userName, Email email, string passwordHash)
    {
        List<Error> errors = new();

        if (string.IsNullOrWhiteSpace(userName))
        {
            errors.Add(Error.Validation("User.EmptyUserName", "Username cannot be empty."));
        }
        else if (userName.Length < 3 || userName.Length > 50)
        {
            errors.Add(Error.Validation("User.InvalidUserNameLength", "Username must be between 3 and 50 characters."));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            errors.Add(Error.Validation("User.EmptyPasswordHash", "Password hash cannot be empty."));
        }

        return errors.Count > 0
            ? errors
            : new User(Guid.NewGuid(), userName, email, passwordHash);
    }
}
