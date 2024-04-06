# Снятие группы заявок

Для снятия сразу нескольких заявок можно использовать метод [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders(System.Nullable{System.Boolean},StockSharp.BusinessEntities.Portfolio,System.Nullable{StockSharp.Messages.Sides},StockSharp.BusinessEntities.ExchangeBoard,StockSharp.BusinessEntities.Security,System.Nullable{StockSharp.Messages.SecurityTypes},System.Nullable{System.Int64}))**(**[System.Nullable\<System.Boolean\>](xref:System.Nullable`1) isStopOrder, [StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [System.Nullable\<StockSharp.Messages.Sides\>](xref:System.Nullable`1) direction, [StockSharp.BusinessEntities.ExchangeBoard](xref:StockSharp.BusinessEntities.ExchangeBoard) board, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [System.Nullable\<StockSharp.Messages.SecurityTypes\>](xref:System.Nullable`1) securityType, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) transactionId **)**, который по маске переданных параметров снимает активные заявки. 

## Примеры снятия группы заявок

Снять все обычные ([OrderTypes.Limit](xref:StockSharp.Messages.OrderTypes.Limit)) заявки для заданного портфеля и инструмента:

```cs
_connector.CancelOrders(false, MainWindow.Instance.Portfolio, null, null, security);
```

Снять все заявки для заданного инструмента: 

```cs
_connector.CancelOrders(null, null, null, null, security);
```

Снять все стоп\-заявки на покупку: 

```cs
_connector.CancelOrders(true, null, Sides.Buy, null, null);
```
