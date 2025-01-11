using Discounts.Application.Exceptions;
using Discounts.Application.Handlers;
using Discounts.Grpc;
using FluentAssertions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Discounts.Tests.Services;

public class DiscountServiceTests
{
    private readonly ICommandHandler<GenerateDiscountCodesRequest, DiscountCodesResponse> _generateCodesHandler;
    private readonly ICommandHandler<UseCodeRequest, UseCodeResponse> _useCodeHandler;
    private readonly ILogger<Discounts.Services.DiscountService> _logger;
    private readonly Discounts.Services.DiscountService _service;

    public DiscountServiceTests()
    {
        _generateCodesHandler = Substitute.For<ICommandHandler<GenerateDiscountCodesRequest, DiscountCodesResponse>>();
        _useCodeHandler = Substitute.For<ICommandHandler<UseCodeRequest, UseCodeResponse>>();
        _logger = Substitute.For<ILogger<Discounts.Services.DiscountService>>();
        _service = new(_generateCodesHandler, _useCodeHandler, _logger);
    }

    [Fact]
    public async Task GenerateDiscountCodes_ShouldReturnResponse_WhenHandlerSucceeds()
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = 10, Length = 8 };
        var response = new DiscountCodesResponse();
        _generateCodesHandler.Handle(request, Arg.Any<CancellationToken>()).Returns(response);
        var context = new ServerCallContextMock();

        // Act
        var result = await _service.GenerateDiscountCodes(request, context);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task GenerateDiscountCodes_ShouldThrowUnexpectedErrorException_WhenHandlerFails()
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = 10, Length = 8 };
        _generateCodesHandler.Handle(request, Arg.Any<CancellationToken>()).Throws(new Exception());
        var context = new ServerCallContextMock();

        // Act
        Func<Task> act = async () => await _service.GenerateDiscountCodes(request, context);

        // Assert
        await act.Should().ThrowAsync<UnexpectedErrorException>();
    }

    [Fact]
    public async Task UseCode_ShouldReturnResponse_WhenHandlerSucceeds()
    {
        // Arrange
        var request = new UseCodeRequest { Code = "VALIDCODE" };
        var response = new UseCodeResponse();
        _useCodeHandler.Handle(request, Arg.Any<CancellationToken>()).Returns(response);
        var context = new ServerCallContextMock();

        // Act
        var result = await _service.UseCode(request, context);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task UseCode_ShouldThrowUnexpectedErrorException_WhenHandlerFails()
    {
        // Arrange
        var request = new UseCodeRequest { Code = "VALIDCODE" };
        _useCodeHandler.Handle(request, Arg.Any<CancellationToken>()).Throws(new Exception());
        var context = new ServerCallContextMock();

        // Act
        Func<Task> act = async () => await _service.UseCode(request, context);

        // Assert
        await act.Should().ThrowAsync<UnexpectedErrorException>();
    }
}

// Mock for ServerCallContext
public class ServerCallContextMock : ServerCallContext
{
    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders) => Task.CompletedTask;
    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions options) => null;
    protected override string MethodCore => "TestMethod";
    protected override string HostCore => "localhost";
    protected override string PeerCore => "localhost";
    protected override DateTime DeadlineCore => DateTime.UtcNow.AddMinutes(1);
    protected override Metadata RequestHeadersCore => new Metadata();
    protected override CancellationToken CancellationTokenCore => CancellationToken.None;
    protected override Metadata ResponseTrailersCore => new Metadata();
    protected override Status StatusCore { get; set; }
    protected override WriteOptions WriteOptionsCore { get; set; }
    protected override AuthContext AuthContextCore => null;
    protected override IDictionary<object, object> UserStateCore => new Dictionary<object, object>();
}
