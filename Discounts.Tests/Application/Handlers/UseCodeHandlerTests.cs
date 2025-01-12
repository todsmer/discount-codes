using Discounts.Application.Entities;
using Discounts.Application.Exceptions;
using Discounts.Application.Handlers;
using Discounts.Application.Repositories;
using Discounts.Grpc;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace Discounts.Tests.Application.Handlers;

public class UseCodeHandlerTests
{
    private readonly IValidator<UseCodeRequest> _validator;
    private readonly IDiscountCodeRepository _repository;
    private readonly UseCodeHandler _handler;

    public UseCodeHandlerTests()
    {
        _validator = Substitute.For<IValidator<UseCodeRequest>>();
        _repository = Substitute.For<IDiscountCodeRepository>();
        _handler = new(_validator, _repository);
    }

    [Fact]
    public async Task Handle_ShouldThrowRequestValidationException_WhenValidationFails()
    {
        // Arrange
        var request = new UseCodeRequest { Code = "INVALID" };
        var validationResult = new FluentValidation.Results.ValidationResult(
            [new FluentValidation.Results.ValidationFailure("Code", "Invalid code")]);
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(validationResult);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenCodeDoesNotExist()
    {
        // Arrange
        var request = new UseCodeRequest { Code = "NON_EXISTENT" };
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _repository.GetUnusedByCode(request.Code, Arg.Any<CancellationToken>())
            .Returns((DiscountCode?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenCodeIsValid()
    {
        // Arrange
        var request = new UseCodeRequest { Code = "VALID" };
        var discountCode = new DiscountCode("VALID");
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _repository.GetUnusedByCode(request.Code, Arg.Any<CancellationToken>())
            .Returns(discountCode);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Result.Should().BeTrue();
        await _repository.Received(1).SaveChanges(Arg.Any<CancellationToken>());
    }
}
