# 类型转换

类型转换组件在确保 StockSharp 中使用的数据类型与特定交易所的格式兼容方面起着重要作用。

## 主要功能

1. 将 StockSharp 类型（e.g.、[Sides](xref:StockSharp.Messages.Sides)、[OrderTypes](xref:StockSharp.Messages.OrderTypes)、[TimeInForce](xref:StockSharp.Messages.TimeInForce)）转换为交易所使用的字符串表示。
2. 将从交易所接收的数据逆向转换为 StockSharp 类型。
3. 在 StockSharp 和交易所格式之间转换工具标识符。
4. 转换时间格式和时间段。

## 实现示例

下面是一个带有类型转换扩展方法的类的示例：

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

## 推荐

- 使用扩展方法方便地使用转换函数。
- 处理所有可能的枚举值，包括 `null` 和未知值。
- 使用 `switch` 表达式（C# 8.0+）以获得更简洁、更可读的代码。
- 添加对无效值的检查，并抛出带有清晰错误信息的异常。
- 考虑使用字典来映射值，特别是对于复杂或经常变化的映射（e.g.，用于时间框架）。

正确实现类型转换可以显著简化在连接器其他部分处理数据的工作，并减少与格式不匹配相关的错误的可能性。