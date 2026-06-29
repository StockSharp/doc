# 订单更换

在创建交易算法时，替换订单比取消后重新注册订单是一种更高级的方法。要替换订单，你应该调用 [Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)** 方法。

作为订单替换的结果，会创建一个新的 [Order](xref:StockSharp.BusinessEntities.Order) 对象，其中包含旧订单信息以及更改的部分。随后，如果你想操作已更改的订单（例如，取消它或再次更改它），你必须使用这个新的 [Order](xref:StockSharp.BusinessEntities.Order) 对象。

下面的例子展示了如何以最佳价格“移动”订单：

```cs
if (registeredOrder.Security.BestBid != null && registeredOrder.Security.BestAsk != null)
{
	// registeredOrder - successfully registered order.
	var newOrder = registeredOrder.Clone();
	// changing the price to be the best on order book
	newOrder.Price = (registeredOrder.Direction == Sides.Buy ? registeredOrder.Security.BestBid : registeredOrder.Security.BestAsk).Price;
	// sending request the replace our order with new price
	_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## 推荐内容

[订单取消](order_cancel.md)
