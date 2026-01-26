using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using AbsoluteCinema.Application.Common;
using MediatR;

namespace AbsoluteCinema.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}