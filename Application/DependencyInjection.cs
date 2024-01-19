using Application.Cache;
using Application.Cache.Interfaces;
using Application.Clients.Options;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.
            AddSingleton<ICache, InMemoryCache>().
            AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}