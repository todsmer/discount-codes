using System.Collections;
using Discounts.Application.Entities;
using Discounts.Application.Repositories;
using Discounts.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Discounts.Infrastructure.Repositories;

internal class DiscountCodeRepository(DiscountDbContext context) : IDiscountCodeRepository
{
    private readonly DbSet<DiscountCode> _dbSet = context.Set<DiscountCode>();

    public async Task<DiscountCode?> GetUnusedByCode(string code, CancellationToken cancellationToken)
        => _dbSet.Local.SingleOrDefault(x => x.Code == code && !x.IsUsed)
            ?? await _dbSet.SingleOrDefaultAsync(x => x.Code == code && !x.IsUsed, cancellationToken);

    public Task<bool> Exists(string code, CancellationToken cancellationToken)
        => _dbSet.Local.Any(x => x.Code == code)
            ? Task.FromResult(true)
            : _dbSet.AnyAsync(x => x.Code == code, cancellationToken);

    public async Task AddMany(IEnumerable<DiscountCode> discountCodes, CancellationToken cancellationToken)
    {
        _dbSet.AddRange(discountCodes);

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChanges(CancellationToken cancellationToken)
        => context.SaveChangesAsync(cancellationToken);

    public Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken)
        => context.Database.BeginTransactionAsync(cancellationToken);
}
