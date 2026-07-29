using System.Data;
using FluentValidation;
using KinematicsSimulator.Application.Features.Simulations.Queries;

namespace KinematicsSimulator.Application.Validators.SimulationValidators;

public class GetSimulationsQueryValidator : AbstractValidator<GetSimulationsQuery>
{
    public GetSimulationsQueryValidator()
    {
        RuleFor(q => q.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty");

        RuleFor(q => q.PageNumber)
            .NotEmpty().WithMessage("Page number cannot be empty")
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(q => q.PageSize)
            .NotEmpty().WithMessage("Page size cannot be empty")
            .InclusiveBetween(1, 50).WithMessage("Page size must be in a safe range of 50 elements");
    }
}
