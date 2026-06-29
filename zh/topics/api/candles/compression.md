# 将逐笔成交和价差压缩为蜡烛

## 简介

API 提供了将逐笔成交数据和价差（最佳买价／卖价）压缩为蜡烛的强大工具。此功能尤其适合分析历史数据和构建自定义指标。

数据压缩的主要扩展方法位于 `CandleHelper` 类中。该类的完整源代码[可在 GitHub 查看](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs)。

建议阅读此文件，以全面了解所有可用方法及其参数。

## 压缩方法

### 将逐笔成交压缩为蜡烛

```cs
// Example usage of ToCandles for ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// This code loads tick data from storage and converts it into candles.
// mdMsg - the message with parameters of the created candles (type, time frame, etc.).
// candleBuilderProvider - the provider that supplies a specific candle builder implementation.
```

### 将价差数据压缩为蜡烛

```cs
// Example usage of ToCandles for spread data
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Here we load spread data and convert it into candles.
// Level1Fields.SpreadMiddle indicates using the spread middle price for building candles.
// You can also use Level1Fields.BestBid or Level1Fields.BestAsk for the best bid or ask prices, respectively.
```

## 压缩参数

压缩数据时可以指定以下参数：

- `series`：定义所创建蜡烛类型和参数的蜡烛序列。
- `type`：用于形成蜡烛的数据类型，例如最佳买价、最佳卖价或价差中间价。
- `candleBuilderProvider`：蜡烛构建器提供程序（可选参数）。

## 使用示例

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ... (initialization code)

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ... (code for building candles from order log)

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (other cases)
	}
}

// This method demonstrates various ways to build candles depending on the type of source data.
// It supports building from ticks, order log, spreads, and other sources.
```

## 其他功能

### 使用不同数据源构建蜡烛

API 不仅可以使用逐笔成交和价差构建蜡烛，还支持其他数据源：

```cs
// Example of building candles from various sources
switch (type)
{
	case BuildTypes.Ticks:
		// ... (code for ticks)

	case BuildTypes.OrderLog:
		// ... (code for order log)

	case BuildTypes.Depths:
		// ... (code for spreads)

	case BuildTypes.Level1:
		// ... (code for Level1)

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.LoadAsync(from, to);

	// ... (other cases)
}

// This code shows how to build candles from different data sources: ticks, order log, spreads, Level1 data, and even from smaller time frame candles.
```

## 结论

API 中的数据压缩方法为处理市场数据提供了灵活工具，可以高效地将逐笔成交和价差数据转换为不同类型、不同时间周期的蜡烛。这对市场分析和交易策略开发尤其有用。
