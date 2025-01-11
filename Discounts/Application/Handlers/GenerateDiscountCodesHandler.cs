using Discounts.Application.Exceptions;
using Discounts.Application.Generators;
using Discounts.Application.Repositories;
using Discounts.Grpc;
using FluentValidation;

namespace Discounts.Application.Handlers;

internal class GenerateDiscountCodesHandler(
    IValidator<GenerateDiscountCodesRequest> validator,
    IDiscountCodeRepository repository,
    IDiscountCodeGenerator codeGenerator
) : ICommandHandler<GenerateDiscountCodesRequest, DiscountCodesResponse>
{
    private const int DuplicateLimit = 5;

    public async Task<DiscountCodesResponse> Handle(GenerateDiscountCodesRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new RequestValidationException(validationResult.ToString());

        var index = 0;
        var duplicateCount = 0;
        var generatedCodes = new List<string>();
        do
        {
            var code = codeGenerator.GenerateCode(request.Length);
            if (await repository.Exists(code, cancellationToken))
            {
                ++duplicateCount;
                if (duplicateCount > DuplicateLimit)
                    throw new InvalidOperationException("Too many duplicate codes generated");

                continue;
            }

            generatedCodes.Add(code);
            repository.Add(new(code: code));

            ++index;

        } while (index < request.Count);

        await repository.SaveChanges(cancellationToken);

        var response = new DiscountCodesResponse();
        response.Codes.AddRange(generatedCodes);

        return response;
    }
}
