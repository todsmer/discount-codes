using Discounts.Application.Repositories;
using Discounts.Configuration;
using Discounts.Infrastructure.Database;
using Discounts.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Discounts.Infrastructure.Bootstrap;

public static class BootstrapExtensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.ConfigureDatabaseSettings();
        applicationBuilder.Services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();
        applicationBuilder.Services.AddDbContext<DiscountDbContext>((sp, options)
            => options.UseSqlServer(sp.GetDatabaseConfiguration().ConnectionString));

        return applicationBuilder;
    }

    private static DatabaseConfiguration GetDatabaseConfiguration(this IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;

    public static async Task MigrateDatabase(this WebApplication application, CancellationToken cancellationToken = default)
    {
        var configuration = application.Services.GetDatabaseConfiguration();
        if (!configuration.Migrate)
            return;

        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    // this would be a generic method in a real environment
    private static IHostApplicationBuilder ConfigureDatabaseSettings(this IHostApplicationBuilder applicationBuilder)
    {
        var section = applicationBuilder.Configuration.GetSection(nameof(DatabaseConfiguration))
                      ?? throw new InvalidOperationException($"Settings section {nameof(DatabaseConfiguration)} not found");

        applicationBuilder.Services.AddOptionsWithValidateOnStart<DatabaseConfiguration>().Bind(section);
        applicationBuilder.Services.AddSingleton<IValidateOptions<DatabaseConfiguration>, DatabaseConfigurationValidator>();

        return applicationBuilder;
    }
}
