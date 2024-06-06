using Puma.Refund.API.Domain.Repositories;

namespace Puma.Refund.API.Extensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IRefundDataRepository, RefundDataRepository>();

        return services;
    }
}
