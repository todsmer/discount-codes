using Discounts.Application.Validators;
using Discounts.Grpc;
using FluentValidation.TestHelper;

namespace Discounts.Tests.Application.Validators;

public class GenerateDiscountCodesRequestValidatorTests
{
    private readonly GenerateDiscountCodesRequestValidator _validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(2001)]
    public void Should_HaveValidationError_When_CountIsOutOfRange(int count)
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = count, Length = 8 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2000)]
    public void Should_NotHaveValidationError_When_CountIsWithinRange(int count)
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = count, Length = 8 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(9)]
    [InlineData(101)]
    public void Should_HaveValidationError_When_LengthIsOutOfRange(int length)
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = 10, Length = length };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Length);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(8)]
    public void Should_NotHaveValidationError_When_LengthIsWithinRange(int length)
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = 10, Length = length };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Length);
    }
}
