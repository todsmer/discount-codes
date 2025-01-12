using Discounts.Application.Entities;
using Discounts.Application.Exceptions;
using Discounts.Application.Generators;
using Discounts.Application.Repositories;
using Discounts.Grpc;
using FluentValidation;
using Polly;

namespace Discounts.Application.Handlers;

internal class GenerateDiscountCodesHandler(
    IValidator<GenerateDiscountCodesRequest> validator,
    IDiscountCodeRepository repository,
    IDiscountCodeGenerator codeGenerator
) : ICommandHandler<GenerateDiscountCodesRequest, DiscountCodesResponse>
{
    private const int BatchSize = 200;
    private const int TotalRetries = 5;
    private static readonly AsyncPolicy RetryPolicy = Policy
        .Handle<Exception>()
        .RetryAsync(TotalRetries);

    public async Task<DiscountCodesResponse> Handle(GenerateDiscountCodesRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new RequestValidationException(validationResult.ToString());

        var totalBatches = GetBatchCount(request.Count);

        var transaction = await repository.BeginTransaction(cancellationToken);
        try
        {
            var response = new DiscountCodesResponse();
            for (var i = 0; i < totalBatches; i++)
            {
                var batchSize = GetBatchSize(request.Count, i);
                var generatedCodes =
                    await RetryPolicy.ExecuteAsync(() => GenerateAndSaveBatch(batchSize, request.Length, cancellationToken));
                response.Codes.AddRange(generatedCodes);
            }

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<IReadOnlyCollection<string>> GenerateAndSaveBatch(int batchSize, int codeLength, CancellationToken cancellationToken)
    {
        var generatedCodes = codeGenerator.GenerateCodes(batchSize, codeLength, cancellationToken);

        var discountCodes = generatedCodes.Select(code => new DiscountCode(code));
        await repository.AddMany(discountCodes, cancellationToken);

        return generatedCodes;
    }

    private static int GetBatchCount(int count)
        => (int)Math.Ceiling((double)count / BatchSize);

    private static int GetBatchSize(int count, int batchIndex)
        => Math.Min(BatchSize, count - batchIndex * BatchSize);
}
