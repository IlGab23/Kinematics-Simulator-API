using FluentValidation;
using KinematicsSimulator.Application.Features.Simulations.Queries;
using KinematicsSimulator.Domain.Entities;

namespace KinematicsSimulator.Application.Validators.SimulationValidators;

public class GetSimulationsQueryValidator : AbstractValidator<GetSimulationsQuery>
{
    public GetSimulationsQueryValidator()
    {
        RuleFor(q => q.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty");

    }
}
