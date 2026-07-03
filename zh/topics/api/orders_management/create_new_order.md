# 创建新订单

要创建一个新订单，你应该创建一个包含订单信息的 [Order](xref:StockSharp.BusinessEntities.Order) 对象，并将其在交易所注册。此外，如果你想操作该订单（例如取消或修改），就应该使用这个 [Order](xref:StockSharp.BusinessEntities.Order) 对象。要在交易所注册订单，请使用 [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** 方法，该方法会将订单发送到服务器。

下面的示例展示了创建订单并在交易所注册的过程：

```cs
	var order = new Order
	{
		Type = OrderTypes.Limit,
		Portfolio = Portfolio.SelectedPortfolio,
		Volume = Volume.Text.To<decimal>(),
		Price = Price.Text.To<decimal>(),
		Security = Security,
		Direction = Sides.Buy,
	};
	_connector.RegisterOrder(order);
	
```

## 推荐内容

[取消订单](order_cancel.md)
