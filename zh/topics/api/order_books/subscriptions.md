# 订阅

## 订阅订单簿

要订阅 StockSharp 的订单簿，您需要执行以下步骤：

1. 订阅事件以接收订单簿 [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) 并处理 [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) 接口对象：

```cs
// 事件处理器
private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// 这里可以处理订单簿数据，例如显示在屏幕上或用于交易策略
	Console.WriteLine($"Received order book for {orderBook.SecurityId}. Best buy price: {orderBook.GetBestBid()?.Price}, Best sell price: {orderBook.GetBestAsk()?.Price}");
}

// 订阅事件
connector.OrderBookReceived += OnOrderBookReceived;
```

在发送订单簿订阅请求之前，**订阅** [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) 事件非常重要。这可以确保如果订单簿在订阅请求发送后很快开始到达，你不会错过任何数据。

2. 使用 [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)) 方法发送订阅请求：

```cs
var security = GetSecurity(); // Get the Security object you want to subscribe to
				
// 订阅订单簿
var subscription = new Subscription(DataType.MarketDepth, security);
connector.Subscribe(subscription);
```

## 取消订阅订单簿

要取消订阅订单簿，请调用 [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe(StockSharp.BusinessEntities.Subscription)) 方法：

```cs
connector.UnSubscribe(subscription);
```

## 关于接收订单簿的说明

在处理 [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) 事件时，重要的是要理解，通过此事件传来的订单簿已经被整理好并且可以直接使用。这意味着，无论数据源的传输方式如何——无论是差异数据（只包含订单簿的变化）还是订单簿的完整快照——StockSharp 平台处理这些数据的方式，确保交易者收到的是完整且更新的订单簿。

该平台会自动将变动整合到订单簿中，在调用 [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) 事件之前更新其内容到当前状态。这简化了数据处理工作，因为交易者无需独立处理差异数据或从连续快照中编译订单簿。因此，你可以确信在事件处理器中收到的数据反映了事件发生时订单簿的最新状态。

这显著简化了交易策略和市场分析的开发，因为交易者可以直接专注于策略逻辑，而无需花费时间在编译和处理订单簿数据的技术细节上。

## 使用示例

使用订单簿的示例可在 *Samples/01_Basic/02_MarketDepths* 项目中找到，位于 [GitHub](https://github.com/StockSharp/StockSharp/)，或者在 StockSharp API 存档中获取，该存档可以通过 [Installer](../../installer.md) 获得。这些示例提供了连接交易系统、订阅过滤后的订单簿以及处理接收数据的实用演示，可以作为开发您自己的交易策略的良好起点。

## 另请参阅

[订阅](../market_data/subscriptions.md)
