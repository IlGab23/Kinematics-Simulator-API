using FluentValidation;
using KinematicsSimulator.Application.Features.Simulations.Commands;

namespace KinematicsSimulator.Application.Validators.SimulationValidators;

public class CreateSimulationCommandValidator : AbstractValidator<CreateSimulationCommand>
{
    public CreateSimulationCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(c => c.SimType)
            .NotEmpty().WithMessage("Simulation type is required.")
            .MaximumLength(4).WithMessage("Simulation type must not exceed 4 characters.");

        RuleFor(c => c.TargetVariable)
            .NotEmpty().WithMessage("Target variable is required.")
            .MaximumLength(10).WithMessage("Target variable must not exceed 10 characters.");

        RuleFor(c => c.T)
            .GreaterThan(0).When(c => c.T.HasValue)
            .WithMessage("Time (T) must be greater than 0 if provided.");
    }
}
