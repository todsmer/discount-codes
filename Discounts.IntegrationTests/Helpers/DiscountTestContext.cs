using Discounts.Grpc;
using Discounts.Infrastructure.Database;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Discounts.IntegrationTests.Helpers;

public class DiscountTestContext
{
    public DiscountService.DiscountServiceClient GrpcClient { get; }

    public DiscountTestContext()
    {
        var dbName = Guid.NewGuid().ToString();
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DiscountDbContext>();
                    services.RemoveAll<DbContextOptions<DiscountDbContext>>();
                    services.RemoveAll<IDbContextOptionsConfiguration<DiscountDbContext>>();

                    services.AddDbContext<DiscountDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });
                });
            });

        var client = factory.CreateDefaultClient();
        var channel = GrpcChannel.ForAddress(client.BaseAddress!, new() { HttpClient = client });
        GrpcClient = new(channel);
    }
}
