# Снятие заявок

Снятие заявки необходимо в том случае, если ситуация на рынке изменилась не в пользу выставленной заявки. Для снятия заявок в [S\#](../../api.md) предусмотрен метод [Connector.CancelOrder](xref:StockSharp.Algo.Connector.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**. 

```cs
// registeredOrder - это ранее зарегистрированная заявка.
_connector.CancelOrder(registeredOrder);
```

## См. также

[Снятие группы заявок](orders_mass_cancel.md)

[Замена заявок](orders_replacement.md)
