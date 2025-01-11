using Discounts.Grpc;
using Discounts.IntegrationTests.Helpers;
using FluentAssertions;

namespace Discounts.IntegrationTests;

public class DiscountServiceTests(DiscountTestContext context) : IClassFixture<DiscountTestContext>
{
    [Fact]
    public async Task GenerateDiscountCodes_ShouldReturnResponse()
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = 10, Length = 8 };

        // Act
        var response = await context.GrpcClient.GenerateDiscountCodesAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.Codes.Count.Should().Be(10);
        response.Codes.Should().OnlyContain(c => c.Length == 8);
        response.Codes.Distinct().Count().Should().Be(10);
    }

    [Fact]
    public async Task UseCode_ShouldReturnResponse()
    {
        // Arrange
        var generateRequest = new GenerateDiscountCodesRequest { Count = 1, Length = 8 };
        var generateResponse = await context.GrpcClient.GenerateDiscountCodesAsync(generateRequest);
        var code = generateResponse.Codes[0];
        var useRequest = new UseCodeRequest { Code = code };

        // Act
        var useResponse = await context.GrpcClient.UseCodeAsync(useRequest);

        // Assert
        useResponse.Should().NotBeNull()
            .And.Match<UseCodeResponse>(r => r.Result);
    }
}
