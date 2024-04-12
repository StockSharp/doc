# Замена заявок

Замена заявок при написании торговых алгоритмов более продвинутый способ, чем [Снятие заявок](order_cancel.md) и последующая регистрация. Чтобы заменить заявку необходимо вызывать метод и передать объект новой заявки [Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)**. 

В итоге при замене заявок всегда получается новый объект [Order](xref:StockSharp.BusinessEntities.Order), который содержит информацию старой заявки + измененная часть. В дальнейшем, если требуется работа с измененной заявкой (например, отменить ее или опять изменить), то необходимо использовать именно этот новый объект [Order](xref:StockSharp.BusinessEntities.Order). 

В примере ниже показан способ "движения" заявки по лучшей цене:

```cs
if (registeredOrder.Security.BestBid != null && registeredOrder.Security.BestAsk != null)
{
	// registeredOrder - это ранее зарегистрированная заявка.
	var newOrder = registeredOrder.Clone();
	// изменяем цену на лучшую
	newOrder.Price = (registeredOrder.Direction == Sides.Buy ? registeredOrder.Security.BestBid : registeredOrder.Security.BestAsk).Price;
	// заменяем заявку на бирже
	_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## См. также

[Снятие заявок](order_cancel.md)
