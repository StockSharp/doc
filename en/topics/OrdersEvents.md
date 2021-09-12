# Getting orders information

The [IConnector](xref:StockSharp.BusinessEntities.IConnector) events related to orders listed below:

- [ITransactionProvider.NewOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.NewOrder) \- the new order received event. 
- [Connector.OrderCancelFailed](xref:StockSharp.Algo.Connector.OrderCancelFailed) \- the event on errors related to orders cancel. 
- [Connector.OrderChanged](xref:StockSharp.Algo.Connector.OrderChanged) \- the order state change event (cancelled, matched). 
- [Connector.OrderRegisterFailed](xref:StockSharp.Algo.Connector.OrderRegisterFailed) \- the event on errors related to orders registration. 
- [Connector.NewStopOrder](xref:StockSharp.Algo.Connector.NewStopOrder) \- the new stop order received event. 
- [Connector.StopOrderCancelFailed](xref:StockSharp.Algo.Connector.StopOrderCancelFailed) \- the event on errors related to stop orders cancelling. 
- [Connector.StopOrderChanged](xref:StockSharp.Algo.Connector.StopOrderChanged) \- the stop orders state change event. 
- [Connector.StopOrderRegisterFailed](xref:StockSharp.Algo.Connector.StopOrderRegisterFailed) \- the event on errors related to stop orders registration. 

Transactions (orders registration, replacement or cancelling) are sent in asynchronous mode. Asynchronous mode allows the trading program not to wait for transaction delivery confirmation by the exchange and continues to work further. That reduces program wait time and increases the speed of response to the market situation changes. 

To find out in the program when the exchange has assigned the [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) to the order, you need to subscribe for the [Connector.OrderChanged](xref:StockSharp.Algo.Connector.OrderChanged) event (or the [Connector.StopOrderChanged](xref:StockSharp.Algo.Connector.StopOrderChanged) for stop orders). To determine the registration failed the [Connector.OrderRegisterFailed](xref:StockSharp.Algo.Connector.OrderRegisterFailed) event is used (or the [Connector.StopOrderRegisterFailed](xref:StockSharp.Algo.Connector.StopOrderRegisterFailed) for stop orders). 

> [!CAUTION]
> If at the start of the application the previously registered orders have been passed from the connector, they all passed through the [ITransactionProvider.NewOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.NewOrder) events, regardless of their state (except the [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) state). This is because the [Connector.NewOrder](xref:StockSharp.Algo.Connector.NewOrder) event shows the new orders receiving in the program, not the event of order successful registration. 

## Recommended content

[Transaction number](OrdersTransactionId.md)
