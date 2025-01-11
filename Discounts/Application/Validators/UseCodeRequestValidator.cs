using Discounts.Constants;
using Discounts.Grpc;
using FluentValidation;

namespace Discounts.Application.Validators;

internal class UseCodeRequestValidator : AbstractValidator<UseCodeRequest>
{
    public UseCodeRequestValidator()
    {
        RuleFor(x => x.Code)
            .Length(DiscountCodeConstants.MinCodeLength, DiscountCodeConstants.MaxCodeLength)
            .WithMessage($"Code length should be between {DiscountCodeConstants.MinCodeLength} and {DiscountCodeConstants.MaxCodeLength}");
    }
}
