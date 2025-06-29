# Orders replacement

Replacing orders when creating trading algorithms is a more advanced method than canceling them and registering again. To replace the order you should call the [Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)** method.

As a result of order replacement a new [Order](xref:StockSharp.BusinessEntities.Order) object is created, which contains the old order information plus the changed part. Subsequently, if you want to work with the changed order (for example, to cancel it or change it again), you must use this new [Order](xref:StockSharp.BusinessEntities.Order) object.

The following example shows how "to move" the order at the best price:

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

## Recommended content

[Orders cancel](order_cancel.md)
