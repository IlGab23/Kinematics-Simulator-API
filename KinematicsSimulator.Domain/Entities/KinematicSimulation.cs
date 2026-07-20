using KinematicsSimulator.Domain.Entities.ValueObjects;
using KinematicsSimulator.Domain.ResultPattern;

namespace KinematicsSimulator.Domain.Entities;

public sealed class KinematicSimulation
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public User User { get; } = null!;
    public SimulationType SimulationType { get; init; }
    public ResultValue ResultValue { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    private KinematicSimulation(Guid id, Guid userId, SimulationType simulationType, ResultValue resultValue, DateTimeOffset createdAt)
    {
        Id = id;
        UserId = userId;
        SimulationType = simulationType;
        ResultValue = resultValue;
        CreatedAt = createdAt;
    }

    #pragma warning disable CS8618
    private KinematicSimulation() { }

    public static Result<KinematicSimulation> Create(Guid userId, SimulationType simulationType, ResultValue resultValue, DateTimeOffset createdAt)
    {
        List<Error> errors = new();

        if (userId == Guid.Empty)
        {
            errors.Add(Error.Validation("KinematicSimulation.EmptyUserId", "User ID cannot be empty."));
        }

        if (createdAt == default)
        {
            errors.Add(Error.Validation("KinematicSimulation.EmptyCreatedAt", "Created at date cannot be empty."));
        }

        return errors.Count > 0
            ? errors
            : new KinematicSimulation(Guid.NewGuid(), userId, simulationType, resultValue, createdAt);
    }
}
