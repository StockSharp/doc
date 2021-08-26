# Orders and trades loading

At the start of the strategy you may need to load the previously matched orders and trades (for example, when the algorithm has been restarted during the trading session or orders and trades transferred through the night). To do this: 

1. To find those orders to be downloaded into the strategy, and to return them back from the method (for example, to download orders identifiers, if the strategy records every registration through the 

   [Strategy.ProcessNewOrders](../api/StockSharp.Algo.Strategies.Strategy.ProcessNewOrders.html)

    method from the file). 
2. To combine the result with the base method 

   [Strategy.ProcessNewOrders](../api/StockSharp.Algo.Strategies.Strategy.ProcessNewOrders.html)

   . 
3. Once orders are loaded in the strategy, all of their matched trades will be also loaded. This will be done automatically. 

The following example shows the loading of all trades in the strategy: 

### Loading in the strategy previously matched orders and trades

Loading in the strategy previously matched orders and trades

1. To load the previous state of the [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html), you must override [Strategy.ProcessNewOrders](../api/StockSharp.Algo.Strategies.Strategy.ProcessNewOrders.html). All [IConnector.Orders](../api/StockSharp.BusinessEntities.IConnector.Orders.html) and [IConnector.StopOrders](../api/StockSharp.BusinessEntities.IConnector.StopOrders.html) will be received by this method from the [Strategy.OnStarted](../api/StockSharp.Algo.Strategies.Strategy.OnStarted.html). And you should to filter them:

   ```cs
   private bool _isOrdersLoaded;
   private bool _isStopOrdersLoaded;
   		  	
   protected override IEnumerable<Order> ProcessNewOrders(IEnumerable<Order> newOrders, bool isStopOrders)
   {
   	// check if the orders was already loaded
   	if ((!isStopOrders && _isOrdersLoaded) || (isStopOrders && _isStopOrdersLoaded))
   		return base.ProcessNewOrders(newOrders, isStopOrders);
   	return Filter(newOrders);
   }
   ```
2. To implement orders filtering, you must determine the filter criterion. For example, if to save all registered during the strategy work orders in the file, you can create the filter by the transaction number [Order.TransactionId](../api/StockSharp.BusinessEntities.Order.TransactionId.html). If such number is in the file, so the order was registered through this strategy: 

   ```cs
   private IEnumerable<Order> Filter(IEnumerable<Order> orders)
   {
   	// to get the identifiers from text file
   	var transactions = File.ReadAllLines("orders_{0}.txt".Put(Name)).Select(l => l.To<long>()).ToArray();
   	
   	// finding our orders
   	return orders.Where(o => transactions.Contains(o.TransactionId));
   }
   ```
3. A record of orders transaction numbers registering through the strategy can be accomplished by overriding the [Strategy.RegisterOrder](../api/StockSharp.Algo.Strategies.Strategy.RegisterOrder.html) method: 

   ```cs
   protected override void RegisterOrder(Order order)
   {
   	// registering the order
   	base.RegisterOrder(order);
   	
   	// and saving order's transaction id
   	File.AppendAllLines("orders_{0}.txt".Put(Name), new[]{ order.TransactionId.ToString() });
   }
   ```
4. Once orders are loaded in the strategy, all of their matched trades will be also loaded. This will be done automatically. 

## Recommended content
