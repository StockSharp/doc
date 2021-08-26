# Getting orders information

The [IConnector](../api/StockSharp.BusinessEntities.IConnector.html) events related to orders listed below:

- [NewOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.NewOrder.html)

   \- the new order received event. 
- [OrderCancelFailed](../api/StockSharp.Algo.Connector.OrderCancelFailed.html)

   \- the event on errors related to orders cancel. 
- [OrderChanged](../api/StockSharp.Algo.Connector.OrderChanged.html)

   \- the order state change event (cancelled, matched). 
- [OrderRegisterFailed](../api/StockSharp.Algo.Connector.OrderRegisterFailed.html)

   \- the event on errors related to orders registration. 
- [NewStopOrder](../api/StockSharp.Algo.Connector.NewStopOrder.html)

   \- the new stop order received event. 
- [StopOrderCancelFailed](../api/StockSharp.Algo.Connector.StopOrderCancelFailed.html)

   \- the event on errors related to stop orders cancelling. 
- [StopOrderChanged](../api/StockSharp.Algo.Connector.StopOrderChanged.html)

   \- the stop orders state change event. 
- [StopOrderRegisterFailed](../api/StockSharp.Algo.Connector.StopOrderRegisterFailed.html)

   \- the event on errors related to stop orders registration. 

Transactions (orders registration, replacement or cancelling) are sent in asynchronous mode. Asynchronous mode allows the trading program not to wait for transaction delivery confirmation by the exchange and continues to work further. That reduces program wait time and increases the speed of response to the market situation changes. 

To find out in the program when the exchange has assigned the [Order.Id](../api/StockSharp.BusinessEntities.Order.Id.html) to the order, you need to subscribe for the [Connector.OrderChanged](../api/StockSharp.Algo.Connector.OrderChanged.html) event (or the [Connector.StopOrderChanged](../api/StockSharp.Algo.Connector.StopOrderChanged.html) for stop orders). To determine the registration failed the [Connector.OrderRegisterFailed](../api/StockSharp.Algo.Connector.OrderRegisterFailed.html) event is used (or the [Connector.StopOrderRegisterFailed](../api/StockSharp.Algo.Connector.StopOrderRegisterFailed.html) for stop orders). 

> [!CAUTION]
> If at the start of the application the previously registered orders have been passed from the connector, they all passed through the [NewOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.NewOrder.html) events, regardless of their state (except the [Failed](../api/StockSharp.Messages.OrderStates.Failed.html) state). This is because the [NewOrder](../api/StockSharp.Algo.Connector.NewOrder.html) event shows the new orders receiving in the program, not the event of order successful registration. 

## Recommended content

[Transaction number](OrdersTransactionId.md)
