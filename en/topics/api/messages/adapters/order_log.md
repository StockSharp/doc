# Order log

If connection supports the request log, then the adapter can transmit it via the [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) message:

```cs
			SendOutMessage(new ExecutionMessage
			{
				ExecutionType = ExecutionTypes.OrderLog, // <- setting the flag that the message contains olive OL
				ServerTime = order.TradeTime,
				SecurityId = order.Symbol.ToStockSharp(section),
				Side = order.Side.ToSide(),
				OrderPrice = (decimal)order.Price,
				OrderType = order.Type.ToOrderType(out var postOnly, out _),
				OrderState = order.Status.ToOrderState(),
				OrderVolume = order.Quantity,
				Balance = (decimal?)(order.Quantity - order.AccumFilled),
				TimeInForce = order.Tif.ToTimeInForce(out var postOnly2),
				AveragePrice = (decimal?)order.AveragePrice,
			});
```

If support for order book generation from incoming data is required, then the [OrderLogMessageAdapter](xref:StockSharp.Algo.OrderLogMessageAdapter) adapter (see [Adapters chain](adapters_chain.md) for details) will be used for subscriptions that require order book collection from order log data. By default, the [OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder) algorithm will be used. If this class behavior is not enough, then you need to create your own class that implements the [IOrderLogMarketDepthBuilder](xref:StockSharp.Messages.IOrderLogMarketDepthBuilder) interface and override the [IMessageAdapter.CreateOrderLogMarketDepthBuilder](xref:StockSharp.Messages.IMessageAdapter.CreateOrderLogMarketDepthBuilder(StockSharp.Messages.SecurityId))**(**[StockSharp.Messages.SecurityId](xref:StockSharp.Messages.SecurityId) securityId **)**: method: 

```cs
public partial class MyOwnMessageAdapter : MessageAdapter
{
	// ...
	
	/// <inheritdoc />
	public IOrderLogMarketDepthBuilder CreateOrderLogMarketDepthBuilder(SecurityId securityId) => new MyOwnOrderLogMarketDepthBuilder();
}
```
