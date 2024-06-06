using Carter;

namespace Puma.Refund.Extensions.EndpointModules;

public static class EndpointModulesExtensions
{
    public static IServiceCollection AddEndpointModuleExtensions(this IServiceCollection services)
    {
        services.AddCarter();

        return services;
    }

    public static WebApplication MapEndpointModules(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}
