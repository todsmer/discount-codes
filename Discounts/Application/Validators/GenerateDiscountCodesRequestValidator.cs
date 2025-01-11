using Discounts.Constants;
using Discounts.Grpc;
using FluentValidation;

namespace Discounts.Application.Validators;

internal class GenerateDiscountCodesRequestValidator : AbstractValidator<GenerateDiscountCodesRequest>
{
    private const int MinCount = 1;
    private const int MaxCount = 2000;

    public GenerateDiscountCodesRequestValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(MinCount)
            .LessThanOrEqualTo(MaxCount)
            .WithMessage($"Count should be between {MinCount} and {MaxCount}");

        RuleFor(x => x.Length)
            .GreaterThanOrEqualTo(DiscountCodeConstants.MinCodeLength)
            .LessThanOrEqualTo(DiscountCodeConstants.MaxCodeLength)
            .WithMessage($"Code length should be between {DiscountCodeConstants.MinCodeLength} and {DiscountCodeConstants.MaxCodeLength}");

    }
}
