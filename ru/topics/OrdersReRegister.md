# Замена заявок

Замена заявок при написании торговых алгоритмов более продвинутый способ, чем [Снятие заявок](OrdersCancel.md) и последующая регистрация. Чтобы заменить заявку необходимо вызывать метод и передать объект новой заявки [Connector.ReRegisterOrder](../api/StockSharp.Algo.Connector.ReRegisterOrder.html). 

В итоге при замене заявок всегда получается новый объект [Order](../api/StockSharp.BusinessEntities.Order.html), который содержит информацию старой заявки + измененная часть. В дальнейшем, если требуется работа с измененной заявкой (например, отменить ее или опять изменить), то необходимо использовать именно этот новый объект [Order](../api/StockSharp.BusinessEntities.Order.html). 

В примере ниже показан способ "движения" заявки по лучшей цене:

```cs
if (registeredOrder.Security.BestBid \!\= null && registeredOrder.Security.BestAsk \!\= null)
{
	\/\/ registeredOrder \- это ранее зарегистрированная заявка.
	var newOrder \= registeredOrder.Clone();
	\/\/ изменяем цену на лучшую
	newOrder.Price \= (registeredOrder.Direction \=\= Sides.Buy ? registeredOrder.Security.BestBid : registeredOrder.Security.BestAsk).Price;
	\/\/ заменяем заявку на бирже
	\_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## См. также

[Снятие заявок](OrdersCancel.md)
