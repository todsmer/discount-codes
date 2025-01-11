namespace Discounts.Application.Generators;

public interface IDiscountCodeGenerator
{
    string GenerateCode(int length);
}

internal class DiscountCodeGenerator : IDiscountCodeGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string GenerateCode(int length)
    {
        var random = new Random();
        return new(Enumerable.Repeat(Chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
