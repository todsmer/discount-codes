namespace Discounts.Application.Handlers;

public interface ICommandHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
