# 将逐笔成交和价差压缩为K线

## 简介

API 提供了将逐笔成交数据和价差（最佳买价／卖价）压缩为K线的强大工具。此功能尤其适合分析历史数据和构建自定义指标。

数据压缩的主要扩展方法位于 `CandleHelper` 类中。该类的完整源代码[可在 GitHub 查看](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs)。

建议阅读此文件，以全面了解所有可用方法及其参数。

## 压缩方法

### 将逐笔成交压缩为K线

```cs
// ToCandles 用于 tick 的示例
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// 此代码从存储加载 tick 数据并将其转换为 K线。
// mdMsg - 包含待创建K线参数（类型、时间周期等）的消息。
// candleBuilderProvider — 提供具体 K线构建器实现的提供者。
```

### 将价差数据压缩为K线

```cs
// ToCandles 用于价差数据的示例
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// 这里加载价差数据并将其转换为 K线。
// Level1Fields.SpreadMiddle 表示使用价差中间价构建 K线。
// 也可以分别使用 Level1Fields.BestBid 或 Level1Fields.BestAsk 表示最佳 bid 或 ask 价格。
```

## 压缩参数

压缩数据时可以指定以下参数：

- `series`：定义所创建K线类型和参数的K线序列。
- `type`：用于形成K线的数据类型，例如最佳买价、最佳卖价或价差中间价。
- `candleBuilderProvider`：K线构建器提供程序（可选参数）。

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
			// ...（从订单日志构建 K 线的代码）

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (other cases)
	}
}

// 此方法演示根据源数据类型构建 K线的不同方式。
// 支持从 tick、订单日志、价差和其他来源构建。
```

## 其他功能

### 使用不同数据源构建K线

API 不仅可以使用逐笔成交和价差构建K线，还支持其他数据源：

```cs
// 从不同来源构建 K线的示例
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

// 此代码展示如何从不同数据源构建 K线：tick、订单日志、价差、Level1 数据，甚至更小周期的 K线。
```

## 结论

API 中的数据压缩方法为处理市场数据提供了灵活工具，可以高效地将逐笔成交和价差数据转换为不同类型、不同时间周期的K线。这对市场分析和交易策略开发尤其有用。
