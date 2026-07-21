using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace KinematicsSimulator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assembly));

        return services;
    }
}
