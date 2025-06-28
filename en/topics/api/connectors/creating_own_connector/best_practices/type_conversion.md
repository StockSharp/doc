# Type Conversion

The type conversion component plays an important role in ensuring compatibility between the data types used in StockSharp and the formats specific to a particular exchange.

## Main Functions

1. Converting StockSharp types (e.g., [Sides](xref:StockSharp.Messages.Sides), [OrderTypes](xref:StockSharp.Messages.OrderTypes), [TimeInForce](xref:StockSharp.Messages.TimeInForce)) to string representations used by the exchange.
2. Reverse conversion of data received from the exchange to StockSharp types.
3. Converting instrument identifiers between StockSharp and exchange formats.
4. Converting time formats and timeframes.

## Implementation Example

Below is an example of a class with extension methods for type conversion:

```cs
static class Extensions
{
	// Converting StockSharp order side to exchange string representation
	public static string ToNative(this Sides side)
	{
		return side switch
		{
			Sides.Buy => "buy",
			Sides.Sell => "sell",
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};
	}

	// Converting exchange order side string representation to StockSharp type
	public static Sides ToSide(this string side)
		=> side?.ToLowerInvariant() switch
		{
			"buy" or "bid" => Sides.Buy,
			"sell" or "ask" or "offer" => Sides.Sell,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};

	// Converting StockSharp order type to exchange string representation
	public static string ToNative(this OrderTypes? type)
	{
		return type switch
		{
			null => null,
			OrderTypes.Limit => "limit",
			OrderTypes.Market => "market",
			OrderTypes.Conditional => "stop",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};
	}

	// Converting exchange order type string representation to StockSharp type
	public static OrderTypes ToOrderType(this string type)
		=> type?.ToLowerInvariant() switch
		{
			"limit" => OrderTypes.Limit,
			"market" => OrderTypes.Market,
			"stop" or "stop limit" => OrderTypes.Conditional,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};

	// Other conversion methods...

	// Dictionary for mapping StockSharp timeframes to exchange string representations
	public static readonly PairSet<TimeSpan, string> TimeFrames = new()
	{
		{ TimeSpan.FromMinutes(1), "ONE_MINUTE" },
		{ TimeSpan.FromMinutes(5), "FIVE_MINUTE" },
		// Other timeframes...
	};

	// Converting StockSharp timeframe to exchange string representation
	public static string ToNative(this TimeSpan timeFrame)
		=> TimeFrames.TryGetValue(timeFrame) ?? throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, LocalizedStrings.InvalidValue);

	// Converting exchange timeframe string representation to TimeSpan
	public static TimeSpan ToTimeFrame(this string name)
		=> TimeFrames.TryGetKey2(name) ?? throw new ArgumentOutOfRangeException(nameof(name), name, LocalizedStrings.InvalidValue);
}
```

## Recommendations

- Use extension methods for convenient usage of conversion functions.
- Handle all possible enumeration values, including `null` and unknown values.
- Use `switch` expressions (C# 8.0+) for cleaner and more readable code.
- Add checks for invalid values and throw exceptions with clear error messages.
- Consider using dictionaries for mapping values, especially for complex or frequently changing mappings (e.g., for timeframes).

Proper implementation of type conversion significantly simplifies working with data in other parts of the connector and reduces the likelihood of errors related to format mismatches.