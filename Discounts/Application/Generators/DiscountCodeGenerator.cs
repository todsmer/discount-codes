using System.Collections.Concurrent;

namespace Discounts.Application.Generators;

public interface IDiscountCodeGenerator
{
    IReadOnlyCollection<string> GenerateCodes(int count, int length, CancellationToken cancellationToken);
}

internal class DiscountCodeGenerator : IDiscountCodeGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public IReadOnlyCollection<string> GenerateCodes(int count, int length, CancellationToken cancellationToken)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");

        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero.");

        var codes = new ConcurrentBag<string>();
        Parallel.ForEach(Enumerable.Range(0, count), new() { CancellationToken = cancellationToken }, _ =>
        {
            var code = GenerateCode(length);
            codes.Add(code);
        });

        return codes;
    }

    private static string GenerateCode(int length)
    {
        var random = new Random();
        return new(Enumerable.Repeat(Chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
