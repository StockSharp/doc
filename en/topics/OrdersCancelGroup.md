# Orders mass cancel

To cancel several orders the [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders(System.Nullable{System.Boolean},StockSharp.BusinessEntities.Portfolio,System.Nullable{StockSharp.Messages.Sides},StockSharp.BusinessEntities.ExchangeBoard,StockSharp.BusinessEntities.Security,System.Nullable{StockSharp.Messages.SecurityTypes},System.Nullable{System.Int64})) method used, which cancel the active orders by the parameters mask passed. 

## Orders mass cancel examples

To cancel all ordinary orders ([OrderTypes.Limit](xref:StockSharp.Messages.OrderTypes.Limit)) for the specified portfolio and instrument:

```cs
_connector.CancelOrders(false, MainWindow.Instance.Portfolio, null, null, security);
```

To cancel all orders for the specified instrument: 

```cs
_connector.CancelOrders(null, null, null, null, security);
```

To cancel all long stop orders: 

```cs
_connector.CancelOrders(true, null, Sides.Buy, null, null);
```
