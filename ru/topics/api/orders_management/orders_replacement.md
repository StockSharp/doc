# Замена заявок

Замена заявок при написании торговых алгоритмов является более продвинутым способом, чем [Снятие заявок](order_cancel.md) и последующая регистрация. Чтобы заменить заявку, необходимо вызывать метод [Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)** и передать объект новой заявки.

В итоге при замене заявок всегда получается новый объект [Order](xref:StockSharp.BusinessEntities.Order), который содержит информацию старой заявки + измененная часть. В дальнейшем, если требуется работа с измененной заявкой (например, отменить ее или опять изменить), то необходимо использовать именно этот новый объект [Order](xref:StockSharp.BusinessEntities.Order). 

В примере ниже показан способ "движения" заявки по лучшей цене:

```cs
// registeredOrder - это ранее зарегистрированная заявка.
var newOrder = registeredOrder.Clone();

// Получаем лучшие цены из Level1 данных.
// Примечание: свойства Security.BestBid / Security.BestAsk являются устаревшими (obsolete).
// Рекомендуется использовать подписку на Level1 и получать лучшие цены из событий Level1Received.
var bestBidPrice = _connector.GetSecurityValue(registeredOrder.Security.Id, Level1Fields.BestBidPrice);
var bestAskPrice = _connector.GetSecurityValue(registeredOrder.Security.Id, Level1Fields.BestAskPrice);

if (bestBidPrice != null && bestAskPrice != null)
{
	// изменяем цену на лучшую
	newOrder.Price = registeredOrder.Side == Sides.Buy ? (decimal)bestBidPrice : (decimal)bestAskPrice;
	// заменяем заявку на бирже
	_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## См. также

[Снятие заявок](order_cancel.md)
