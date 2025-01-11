using Discounts.Infrastructure.Database.Entities;

namespace Discounts.Application.Repositories;

public interface IDiscountCodeRepository
{
    Task<DiscountCode?> GetUnusedByCode(string code, CancellationToken cancellationToken);

    Task<bool> Exists(string code, CancellationToken cancellationToken);
    void Add(DiscountCode discountCode);

    Task SaveChanges(CancellationToken cancellationToken);
}
