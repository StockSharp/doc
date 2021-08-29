# Orders mass cancel

To cancel several orders the [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders) method used, which cancel the active orders by the parameters mask passed. 

### Orders mass cancel examples

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
