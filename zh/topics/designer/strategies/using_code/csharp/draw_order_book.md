# 绘制订单簿

代码策略可以在[订单簿](../../../user_interface/components/order_book.md)面板中绘制数据，效果与 [Order Book](../../using_visual_designer/elements/market_depths/order_book_panel.md) 模块相同。为此，需要编写以下代码。

1. 创建 [IOrderBookSource](xref:StockSharp.Algo.Strategies.IOrderBookSource) 接口的实现，**Designer** 使用该接口识别数据源。本例使用 [OrderBookSource](xref:StockSharp.Algo.Strategies.OrderBookSource) 类，即该接口的默认实现：

```cs
private static readonly OrderBookSource _bookSource = new OrderBookSource("SMA");
```

2. 重写 [OrderBookSources](xref:StockSharp.Algo.Strategies.Strategy.OrderBookSources) 属性：

```cs
public override IEnumerable<IOrderBookSource> OrderBookSources
	=> new[] { _bookSource };
```

这样，策略便会向外部代码（本例中为[订单簿](../../../user_interface/components/order_book.md)面板）说明可用的订单簿数据源。如果策略使用多个订单簿，例如不同交易品种的订单簿，或经过不同方式处理的订单簿（如[稀疏订单簿](../../using_visual_designer/elements/market_depths/sparse_order_book.md)），则可以提供多个数据源。

3. 在策略代码中添加订单簿订阅的初始化。对于 SmaStrategy，请将其添加到 [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted) 方法末尾：

```cs
var bookSubscription = new Subscription(DataType.MarketDepth, Security);
			
bookSubscription
	.WhenOrderBookReceived(this)
	.Do(book =>
	{
		// 绘制订单簿
		DrawOrderBook(bookSubscription, _bookSource, book);
	})
	.Apply(this);
			
Subscribe(bookSubscription);
```

在 Do 处理程序中调用 [DrawOrderBook](xref:StockSharp.Algo.Strategies.Strategy.DrawOrderBook(StockSharp.BusinessEntities.Subscription,StockSharp.Algo.Strategies.IOrderBookSource,StockSharp.Messages.IOrderBookMessage)) 方法，将订单簿发送到界面进行绘制。

4. 添加[订单簿](../../../user_interface/components/order_book.md)面板，并选择在代码中创建的数据源：

  ![Designer_Source_Code_OrderBook_00](../../../../../images/designer_source_code_orderbook_00.png)

5. 启动策略测试后，订单簿中会显示数据：

  ![Designer_Source_Code_OrderBook_01](../../../../../images/designer_source_code_orderbook_01.png)
