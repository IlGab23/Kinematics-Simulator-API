using System.Reflection;
using System.Runtime.InteropServices.ObjectiveC;
using FluentValidation;
using KinematicsSimulator.Domain.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KinematicsSimulator.Application.Behavior;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> log)
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        log.LogWarning("Pipeline Behavior ACtivated!");

        if (!validators.Any()) return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResult.SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0) return await next(cancellationToken);

        var errorDetails = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}").ToArray();

        var validationErrors = Error.Validation("Request.Validation", "Request contains validation errors", errorDetails);

        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = typeof(TResponse).GetGenericArguments();
            var resultType = typeof(Result<>).MakeGenericType(valueType);

            var failureMethod = resultType.GetMethod("Failure", BindingFlags.Public | BindingFlags.Static);
            if (failureMethod is not null)
            {
                return (TResponse)failureMethod.Invoke(null, [new List<Error> {validationErrors}])!;
            }
        }
        else if (typeof(TResponse) == typeof(ResultBase))
        {
            return (TResponse)(object)ResultBase.Failure([validationErrors]);
        }

        throw new ValidationException(failures);
    }
}
