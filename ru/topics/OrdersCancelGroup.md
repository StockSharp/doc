# Снятие группы заявок

Для снятия сразу нескольких заявок можно использовать метод [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders), который по маске переданных параметров снимает активные заявки. 

### Примеры снятия группы заявок

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

## См. также
