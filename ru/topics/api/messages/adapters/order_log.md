# Лог заявок

Если подключение поддерживает лог заявок, то адаптер может передавать его через сообщение [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage):

```cs
			SendOutMessage(new ExecutionMessage
			{
				ExecutionType = ExecutionTypes.OrderLog, // <- устанавливаем признак того, что сообщение содержил ОЛ
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

Если требуется поддержка генерации стаканов из приходящих данных, то будет использоваться адаптер [OrderLogMessageAdapter](xref:StockSharp.Algo.OrderLogMessageAdapter) (подробнее [Вспомогательные адаптеры](adapters_chain.md)) при подписке, требущих сбор стаканов из ОЛ данных. По умолчанию, будет использовать алгоритм [OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder). Если поведение данного класса недостаточно, то необходимо создать собственный класс, реализующий интерфейс [IOrderLogMarketDepthBuilder](xref:StockSharp.Messages.IOrderLogMarketDepthBuilder) и переопределить метод [IMessageAdapter.CreateOrderLogMarketDepthBuilder](xref:StockSharp.Messages.IMessageAdapter.CreateOrderLogMarketDepthBuilder(StockSharp.Messages.SecurityId))**(**[StockSharp.Messages.SecurityId](xref:StockSharp.Messages.SecurityId) securityId **)**:

```cs
public partial class MyOwnMessageAdapter : MessageAdapter
{
	// ...
	
	/// <inheritdoc />
	public IOrderLogMarketDepthBuilder CreateOrderLogMarketDepthBuilder(SecurityId securityId) => new MyOwnOrderLogMarketDepthBuilder();
}
```
