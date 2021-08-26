# Снятие заявок

Снятие заявки необходимо в том случае, если ситуация на рынке изменилась не в пользу выставленной заявки. Для снятия заявок в [S\#](StockSharpAbout.md) предусмотрен метод [Connector.CancelOrder](../api/StockSharp.Algo.Connector.CancelOrder.html). 

```cs
\/\/ registeredOrder \- это ранее зарегистрированная заявка.
\_connector.CancelOrder(registeredOrder);
```

## См. также

[Снятие группы заявок](OrdersCancelGroup.md)

[Замена заявок](OrdersReRegister.md)
