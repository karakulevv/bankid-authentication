namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {       
        // nothing to register at the moment
        return services;
    }
}
