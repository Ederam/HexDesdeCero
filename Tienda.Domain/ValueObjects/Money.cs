namespace Tienda.Domain.ValueObjects;

/// <summary>
/// Value object representing a monetary value with its associated currency code.
/// Implemented as a readonly record struct for high performance and zero heap allocation.
/// </summary>
public readonly record struct Money
{
    /// <summary>
    /// Gets the ISO currency code (e.g., "COP", "USD").
    /// </summary>
    public string CurrencyCode { get; }

    /// <summary>
    /// Gets the monetary amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Money"/> struct.
    /// </summary>
    /// <param name="currencyCode">The ISO currency code.</param>
    /// <param name="amount">The monetary amount.</param>
    public Money(string currencyCode, decimal amount)
    {
        CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "COP" : currencyCode;
        Amount = amount;
    }

    /// <summary>
    /// Creates a new <see cref="Money"/> instance in COP currency.
    /// </summary>
    /// <param name="amount">The monetary amount in COP.</param>
    /// <returns>A new <see cref="Money"/> struct instance.</returns>
    public static Money Cop(decimal amount) => new("COP", amount);
}