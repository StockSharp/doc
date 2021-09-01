# Снятие группы заявок

Для снятия сразу нескольких заявок можно использовать метод [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders(System.Nullable{System.Boolean},StockSharp.BusinessEntities.Portfolio,System.Nullable{StockSharp.Messages.Sides},StockSharp.BusinessEntities.ExchangeBoard,StockSharp.BusinessEntities.Security,System.Nullable{StockSharp.Messages.SecurityTypes},System.Nullable{System.Int64})), который по маске переданных параметров снимает активные заявки. 

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
