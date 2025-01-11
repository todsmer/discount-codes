using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Discounts.Services.Bootstrap;

public static class BoostrapExtensions
{
    public static IHostApplicationBuilder AddServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddGrpc();
        return applicationBuilder;
    }
}
