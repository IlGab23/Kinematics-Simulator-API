using KinematicsSimulator.Domain.ResultPattern;

namespace KinematicsSimulator.Domain.Entities.ValueObjects;

public sealed record ResultValue
{
    public double Value { get; init; }

    private ResultValue(double value) => Value = value;
    private ResultValue() { }

    public static Result<ResultValue> Create(double value)
    {
        List<Error> errors = new();

        if (double.IsNaN(value))
        {
            errors.Add(Error.Validation("ResultValue.IsNaN", "Result value cannot be NaN."));
        }
        else if (double.IsInfinity(value))
        {
            errors.Add(Error.Validation("ResultValue.IsInfinity", "Result value cannot be infinite."));
        }

        return errors.Count > 0
            ? errors
            : new ResultValue(value);
    }
}
