# Orders replacement

Orders replacement in the trading algorithms creating is more advanced way than the [Orders cancel](OrdersCancel.md) and the following registration. To replace the order you should call the [Connector.ReRegisterOrder](../api/StockSharp.Algo.Connector.ReRegisterOrder.html) method. 

As the result or orders replacement the new [Order](../api/StockSharp.BusinessEntities.Order.html), object created, which contains old order information plus the changed part. Subsequently, if you want to work with the changed order (for example, to cancel or to change it again), you must use this new [Order](../api/StockSharp.BusinessEntities.Order.html) object. 

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

[Orders cancel](OrdersCancel.md)
