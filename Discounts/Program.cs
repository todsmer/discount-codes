using Discounts.Application.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Discounts.Infrastructure.Bootstrap;
using Discounts.Services.Bootstrap;
using Microsoft.Extensions.Configuration;
using Serilog;
using DiscountService = Discounts.Services.DiscountService;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((_, options)
    => options.ReadFrom.Configuration(builder.Configuration));

builder
    .AddInfrastructure()
    .AddServices()
    .AddApplication();

var app = builder.Build();

await app.MigrateDatabase();

app.MapGrpcService<DiscountService>();

await app.RunAsync();
