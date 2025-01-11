namespace Discounts.Infrastructure.Database.Entities;

public class DiscountCode
{
    // This is the constructor that will be used by the Entity Framework
    #pragma warning disable CS8618, CS9264
    public DiscountCode() {}
    #pragma warning restore CS8618, CS9264

    public DiscountCode(string code)
    {
        Id = Guid.NewGuid();
        Code = code;
        IsUsed = false;
    }

    public Guid Id { get; }
    public string Code { get; }
    public bool IsUsed { get; private set; }

    public void MarkAsUsed()
    {
        IsUsed = true;
    }
}
