using Discounts.Application.Exceptions;
using Discounts.Application.Repositories;
using Discounts.Grpc;
using FluentValidation;

namespace Discounts.Application.Handlers;

internal class UseCodeHandler(
    IValidator<UseCodeRequest> validator,
    IDiscountCodeRepository repository) : ICommandHandler<UseCodeRequest, UseCodeResponse>
{
    public async Task<UseCodeResponse> Handle(UseCodeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new RequestValidationException(validationResult.ToString());

        var discountCode = await repository.GetUnusedByCode(request.Code, cancellationToken);
        if (discountCode is null)
            return new() { Result = false };

        discountCode.MarkAsUsed();

        await repository.SaveChanges(cancellationToken);

        return new() { Result = true };
    }
}
