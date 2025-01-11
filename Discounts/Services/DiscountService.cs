using Discounts.Application.Exceptions;
using Discounts.Application.Handlers;
using Discounts.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discounts.Services;

public class DiscountService(
    ICommandHandler<GenerateDiscountCodesRequest, DiscountCodesResponse> generateCodesHandler,
    ICommandHandler<UseCodeRequest, UseCodeResponse> useCodeHandler,
    ILogger<DiscountService> logger) : Grpc.DiscountService.DiscountServiceBase
{
    public override async Task<DiscountCodesResponse> GenerateDiscountCodes(GenerateDiscountCodesRequest request,
        ServerCallContext context)
    {
        try
        {
            return await generateCodesHandler.Handle(request, context.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while generating discount codes");
            throw new UnexpectedErrorException();
        }
    }

    public override async Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
    {
        try
        {
            return await useCodeHandler.Handle(request, context.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while using a discount code");
            throw new UnexpectedErrorException();
        }
    }
}
