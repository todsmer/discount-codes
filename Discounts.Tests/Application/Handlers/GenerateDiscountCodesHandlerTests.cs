using Discounts.Application.Entities;
using Discounts.Application.Exceptions;
using Discounts.Application.Generators;
using Discounts.Application.Handlers;
using Discounts.Application.Repositories;
using Discounts.Grpc;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace Discounts.Tests.Application.Handlers;

public class GenerateDiscountCodesHandlerTests
{
    private readonly IValidator<GenerateDiscountCodesRequest> _validator;
    private readonly IDiscountCodeRepository _repository;
    private readonly IDiscountCodeGenerator _codeGenerator;
    private readonly GenerateDiscountCodesHandler _handler;

    public GenerateDiscountCodesHandlerTests()
    {
        _validator = Substitute.For<IValidator<GenerateDiscountCodesRequest>>();
        _repository = Substitute.For<IDiscountCodeRepository>();
        _codeGenerator = Substitute.For<IDiscountCodeGenerator>();
        _handler = new(_validator, _repository, _codeGenerator);
    }

    [Fact]
    public async Task Handle_ShouldThrowRequestValidationException_WhenValidationFails()
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = 5, Length = 10 };
        var validationResult = new FluentValidation.Results.ValidationResult(
            new[] { new FluentValidation.Results.ValidationFailure("Count", "Invalid count") });
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(validationResult);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();
    }

    [Fact]
    public async Task Handle_ShouldGenerateUniqueCodes_WhenValidRequest()
    {
        // Arrange
        var request = new GenerateDiscountCodesRequest { Count = 3, Length = 10 };
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _codeGenerator.GenerateCodes(request.Count, request.Length, Arg.Any<CancellationToken>())
            .Returns(new List<string> { "CODE1", "CODE2", "CODE3" });
        _repository.GetUnusedByCode(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((DiscountCode?)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Codes.Should().Contain(new List<string> { "CODE1", "CODE2", "CODE3" });
        await _repository.Received(1).AddMany(Arg.Any<IEnumerable<DiscountCode>>(), Arg.Any<CancellationToken>());
    }
}
