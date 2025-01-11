using Discounts.Application.Generators;
using FluentAssertions;

namespace Discounts.Tests.Application.Generators;

public class DiscountCodeGeneratorTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(10)]
    [InlineData(22)]
    [InlineData(2)]
    [InlineData(14)]
    public void GenerateCode_ShouldReturnCodeOfSpecifiedLength(int length)
    {
        // Arrange
        var generator = new DiscountCodeGenerator();

        // Act
        var code = generator.GenerateCode(length);

        // Assert
        code.Should().NotBeNullOrEmpty();
        code.Length.Should().Be(length);
    }

    [Fact]
    public void GenerateCode_ShouldReturnUniqueCodes()
    {
        // Arrange
        var generator = new DiscountCodeGenerator();
        const int length = 10;

        // Act
        var code1 = generator.GenerateCode(length);
        var code2 = generator.GenerateCode(length);

        // Assert
        code1.Should().NotBeNullOrEmpty();
        code2.Should().NotBeNullOrEmpty();
        code1.Should().NotBe(code2);
    }
}
