using Discounts.Application.Generators;
using FluentAssertions;

namespace Discounts.Tests.Application.Generators;

public class DiscountCodeGeneratorTests
{
    private readonly DiscountCodeGenerator _generator = new();

    [Fact]
    public void GenerateCodes_ShouldGenerateCorrectNumberOfCodes()
    {
        // Arrange
        const int count = 10;
        const int length = 8;

        // Act
        var codes = _generator.GenerateCodes(count, length, CancellationToken.None);

        // Assert
        codes.Should().HaveCount(count);
    }

    [Fact]
    public void GenerateCodes_ShouldGenerateCodesWithCorrectLength()
    {
        // Arrange
        const int count = 10;
        const int length = 8;

        // Act
        var codes = _generator.GenerateCodes(count, length, CancellationToken.None);

        // Assert
        codes.Should().OnlyContain(code => code.Length == length);
    }

    [Fact]
    public void GenerateCodes_ShouldGenerateUniqueCodes()
    {
        // Arrange
        const int count = 100;
        const int length = 8;

        // Act
        var codes = _generator.GenerateCodes(count, length, CancellationToken.None);

        // Assert
        codes.Should().OnlyHaveUniqueItems();
    }
}
