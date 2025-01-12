using Discounts.Grpc;
using Discounts.IntegrationTests.Helpers;
using FluentAssertions;
using Grpc.Core;

namespace Discounts.IntegrationTests;

public class DiscountServiceTests(DiscountTestContext context) : IClassFixture<DiscountTestContext>
{
    [Theory]
    [InlineData(1, 8)]
    [InlineData(1, 7)]
    [InlineData(10, 8)]
    [InlineData(10, 7)]
    [InlineData(2000, 7)]
    [InlineData(2000, 8)]
    [InlineData(1238, 8)]
    [InlineData(449, 7)]
    public async Task GenerateDiscountCodes_ShouldReturnResponse(int count, int length)
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = count, Length = length };

        // Act
        var response = await context.GrpcClient.GenerateDiscountCodesAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.Codes.Count.Should().Be(count);
        response.Codes.Should().OnlyHaveUniqueItems();
        response.Codes.Should().OnlyContain(c => c.Length == length);
    }

    [Theory]
    [InlineData(0, 8)]
    [InlineData(-1, 8)]
    [InlineData(0, 7)]
    [InlineData(-1, 7)]
    [InlineData(10, 0)]
    [InlineData(10, -1)]
    [InlineData(10, 6)]
    [InlineData(10, 9)]
    public async Task GenerateDiscountCodes_ShouldReturnValidationError(int count, int length)
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = count, Length = length };

        // Act
        Func<Task> act = async () => await context.GrpcClient.GenerateDiscountCodesAsync(request);

        // Assert
        await act.Should().ThrowAsync<RpcException>()
            .Where(e => e.StatusCode == StatusCode.InvalidArgument);
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
        useResponse.Should().NotBeNull();
        useResponse.Result.Should().BeTrue();
    }

    [Fact]
    public async Task UseCode_ShouldReturnError_WhenCodeDoesNotExist()
    {
        // Arrange
        var useRequest = new UseCodeRequest { Code = "CODE1234" };

        // Act
        var response = await context.GrpcClient.UseCodeAsync(useRequest);

        // Assert
        response.Result.Should().BeFalse();
    }
}
