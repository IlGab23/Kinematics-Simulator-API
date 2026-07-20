using KinematicsSimulator.Domain.ResultPattern;
namespace KinematicsSimulator.Domain.Entities.ValueObjects;

public sealed record SimulationType
{
    public string Value { get; init; }

    private SimulationType(string name) => Value = name;

    #pragma warning disable CS8618
    private SimulationType() { }

    public static readonly SimulationType MRU = new("MRU");
    public static readonly SimulationType MRUA = new("MRUA");

    public static Result<SimulationType> Create(string inputName)
    {
        List<Error> errors = new();

        if (string.IsNullOrWhiteSpace(inputName)) errors.Add(Error.Validation("SimulationType.Empty", "Simulation type cannot be empty."));

        string normalizedInput = inputName.Trim().ToUpper();

        if(normalizedInput == MRU.Value) return MRU;
        if (normalizedInput == MRUA.Value) return MRUA;

        errors.Add(Error.Validation("SimulationType.WrongType", "Invalid simulation type. Supported types are MRU and MRUA."));

        return errors;
    }
}
