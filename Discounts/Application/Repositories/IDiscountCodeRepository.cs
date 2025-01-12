using Discounts.Application.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Discounts.Application.Repositories;

public interface IDiscountCodeRepository
{
    Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken);
    Task<DiscountCode?> GetUnusedByCode(string code, CancellationToken cancellationToken);

    Task AddMany(IEnumerable<DiscountCode> discountCodes, CancellationToken cancellationToken);

    Task SaveChanges(CancellationToken cancellationToken);
}
