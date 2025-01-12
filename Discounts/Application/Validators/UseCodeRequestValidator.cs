using Discounts.Constants;
using Discounts.Grpc;
using FluentValidation;

namespace Discounts.Application.Validators;

internal class UseCodeRequestValidator : AbstractValidator<UseCodeRequest>
{
    private const string CodePattern = "^[a-zA-Z0-9]*$";

    public UseCodeRequestValidator()
    {
        RuleFor(x => x.Code)
            .Length(DiscountCodeConstants.MinCodeLength, DiscountCodeConstants.MaxCodeLength)
            .WithMessage($"Code length should be between {DiscountCodeConstants.MinCodeLength} and {DiscountCodeConstants.MaxCodeLength}");

        RuleFor(x => x.Code)
            .Matches(CodePattern)
            .WithMessage("Code should contain only alphanumeric characters");
    }
}
