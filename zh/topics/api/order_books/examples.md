# 订单簿示例

## 获得最佳价格

为了从订单簿中获得最佳价格，重要的是关注买单列表（[Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)）和卖单列表（[Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)）的前几个元素，因为这些代表了交易中可获得的最优价格：

```cs
var bestBid = orderBook.Bids.FirstOrDefault();
var bestAsk = orderBook.Asks.FirstOrDefault();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}");
}
```

或者使用现成的扩展方法 [GetBestBid](xref:StockSharp.Messages.Extensions.GetBestBid(StockSharp.Messages.IOrderBookMessage)) 和 [GetBestAsk](xref:StockSharp.Messages.Extensions.GetBestAsk(StockSharp.Messages.IOrderBookMessage))：

```cs
var bestBid = orderBook.GetBestBid();
var bestAsk = orderBook.GetBestAsk();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}, volume: {bestBid.Volume}");
}
else
{
	Console.WriteLine("No best buy orders.");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}, volume: {bestAsk.Volume}");
}
else
{
	Console.WriteLine("No best sell orders.");
}
```

## 分析订单簿深度

要分析订单簿的深度，可以从列表的开头开始，迭代 [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) 和 [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks) 列表中的项目。这可以概览不同价格水平的订单分布，并帮助识别潜在的支撑和阻力水平：

```cs
foreach (var bid in orderBook.Bids)
{
	Console.WriteLine($"Buy price: {bid.Price}, volume: {bid.Volume}");
}

foreach (var ask in orderBook.Asks)
{
	Console.WriteLine($"Sell price: {ask.Price}, volume: {ask.Volume}");
}
```

## 在订单簿中搜索成交量

用于在订单簿中搜索重要成交量的算法有助于识别大订单积聚的水平。这可以表明主要参与者的兴趣，并在做出交易决策时作为额外的信号。

算法：

1. 确定一个将被视为重要的体积阈值。
2. 遍历[Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)和[Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)列表中的订单，将每个订单的数量与阈值进行比较。
3. 记录发现订单量超过阈值的价格水平。

```cs
double significantVolumeThreshold = 10000; // Example of a threshold value

Console.WriteLine("Significant volumes in the order book:");

foreach (var bid in orderBook.Bids)
{
	if (bid.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Buy: Price {bid.Price}, volume {bid.Volume}");
	}
}

foreach (var ask in orderBook.Asks)
{
	if (ask.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Sell: Price {ask.Price}, volume {ask.Volume}");
	}
}
```

该算法有助于突出具有显著交易量的水平，这些水平可能在市场价格变动中发挥关键作用。