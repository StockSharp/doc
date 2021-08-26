# Orders states

The order during it’s life goes through the following states:

![OrderStates](~/images/OrderStates.png)

- [None](../api/StockSharp.Messages.OrderStates.None.html)

   \- the order has been created in the algorithm and has not been sent to the registration. 
- [Pending](../api/StockSharp.Messages.OrderStates.Pending.html)

   \- the order has been sent to the registration (

  [Connector.RegisterOrder](../api/StockSharp.Algo.Connector.RegisterOrder.html)

  ) and the 

  [ITransactionProvider.NewOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.NewOrder.html)

   event for it has been called. The confirmation of the order acceptance from the exchange is expected. If successful, the 

  [Connector.OrderChanged](../api/StockSharp.Algo.Connector.OrderChanged.html)

  , event will be called and the order will be changed to the 

  [Active](../api/StockSharp.Messages.OrderStates.Active.html)

   state. Also the 

  [Order.Id](../api/StockSharp.BusinessEntities.Order.Id.html)

   and 

  [Order.Time](../api/StockSharp.BusinessEntities.Order.Time.html)

   properties will be initialized. In the case of the order rejection the 

  [Connector.OrderRegisterFailed](../api/StockSharp.Algo.Connector.OrderRegisterFailed.html)

   event with the error description will be called and the order will be changed to the 

  [Failed](../api/StockSharp.Messages.OrderStates.Failed.html)

   state. 
- [Active](../api/StockSharp.Messages.OrderStates.Active.html)

   \- the order is active on exchange. Such order will be active as long as all of the order 

  [Order.Volume](../api/StockSharp.BusinessEntities.Order.Volume.html)

   volume is matched, or it will be forcibly cancelled through 

  [ITransactionProvider.CancelOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.CancelOrder.html)

  . If the order matched partially the 

  [ITransactionProvider.NewMyTrade](../api/StockSharp.BusinessEntities.ITransactionProvider.NewMyTrade.html)

   events about new trades on issued order have been called, as well as the 

  [Connector.OrderChanged](../api/StockSharp.Algo.Connector.OrderChanged.html)

   event, which the 

  [Order.Balance](../api/StockSharp.BusinessEntities.Order.Balance.html)

   notification about the order balance change passed. The latest event will be arised in the case of the order cancellation. 
- [Done](../api/StockSharp.Messages.OrderStates.Done.html)

   \- the order is no longer active on the exchange (been fully matched or cancelled). 
- [Failed](../api/StockSharp.Messages.OrderStates.Failed.html)

   \- the order has not been accepted by the exchange (or intermediate system, such as the broker server) for any reason. 

To find out the order trading state (what volume is matched, whether the order fully matched, and so on) the [IsCanceled](../api/StockSharp.Algo.TraderHelper.IsCanceled.html), [IsMatchedEmpty](../api/StockSharp.Algo.TraderHelper.IsMatchedEmpty.html), [IsMatchedPartially](../api/StockSharp.Algo.TraderHelper.IsMatchedPartially.html), [IsMatched](../api/StockSharp.Algo.TraderHelper.IsMatched.html) and [GetMatchedVolume](../api/StockSharp.Algo.TraderHelper.GetMatchedVolume.html) methods should be used: 

```cs
\/\/ any order
Order order \= ....
\/\/ is the order was cancelled
Console.WriteLine(order.IsCanceled());
\/\/ or fully matched
Console.WriteLine(order.IsMatched());
\/\/ or just partially
Console.WriteLine(order.IsMatchedPartially());
\/\/ or non of any contracts was matched 
Console.WriteLine(order.IsMatchedEmpty());
\/\/ so we are getting the realized (\=matched) order size.
Console.WriteLine(order.GetMatchedVolume());
```
