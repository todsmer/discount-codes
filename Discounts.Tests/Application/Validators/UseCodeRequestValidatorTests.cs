using Discounts.Application.Validators;
using Discounts.Grpc;
using FluentValidation.TestHelper;

namespace Discounts.Tests.Application.Validators;

public class UseCodeRequestValidatorTests
{
    private readonly UseCodeRequestValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("thiscodeiswaytoolongtobevalid")]
    public void Should_HaveValidationError_When_CodeLengthIsOutOfRange(string code)
    {
        // Arrange
        var request = new UseCodeRequest { Code = code };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Theory]
    [InlineData("abcdefgh")]
    [InlineData("abcdefg")]
    public void Should_NotHaveValidationError_When_CodeLengthIsWithinRange(string code)
    {
        // Arrange
        var request = new UseCodeRequest { Code = code };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Code);
    }
}
