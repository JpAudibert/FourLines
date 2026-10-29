namespace FourLines.Domain.Models;

public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency)
    {
        ArgumentException.ThrowIfNullOrEmpty(currency, nameof(currency));

        Amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        Currency = currency.ToUpperInvariant();
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add different currencies");

        return new Money(a.Amount + b.Amount, a.Currency);
    }
}
