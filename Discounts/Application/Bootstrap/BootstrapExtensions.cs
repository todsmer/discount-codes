using Discounts.Application.Generators;
using Discounts.Application.Handlers;
using Discounts.Application.Validators;
using Discounts.Grpc;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Discounts.Application.Bootstrap;

public static class BootstrapExtensions
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services
            .AddScoped<ICommandHandler<GenerateDiscountCodesRequest, DiscountCodesResponse>, GenerateDiscountCodesHandler>()
            .AddScoped<ICommandHandler<UseCodeRequest, UseCodeResponse>, UseCodeHandler>()
            .AddSingleton<IValidator<UseCodeRequest>, UseCodeRequestValidator>()
            .AddSingleton<IValidator<GenerateDiscountCodesRequest>, GenerateDiscountCodesRequestValidator>()
            .AddSingleton<IDiscountCodeGenerator, DiscountCodeGenerator>();

        return applicationBuilder;
    }
}
