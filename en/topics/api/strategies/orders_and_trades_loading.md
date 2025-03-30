# Loading Orders and Trades

When starting a strategy, it may be necessary to load previously executed orders and trades (for example, when a robot was restarted during a trading session or when orders and trades are carried over overnight). To do this, you need to:

1. Find the orders that need to be loaded into the strategy and return them from a method (for example, load order IDs if the strategy records them each time during registration through the [Strategy.ProcessNewOrders](xref:StockSharp.Algo.Strategies.Strategy.ProcessNewOrders(System.Collections.Generic.IEnumerable{StockSharp.BusinessEntities.Order}))**(**[System.Collections.Generic.IEnumerable\<StockSharp.BusinessEntities.Order\>](xref:System.Collections.Generic.IEnumerable`1) newOrders **)** method from a file).
2. Combine the obtained result with the base method [Strategy.ProcessNewOrders](xref:StockSharp.Algo.Strategies.Strategy.ProcessNewOrders(System.Collections.Generic.IEnumerable{StockSharp.BusinessEntities.Order}))**(**[System.Collections.Generic.IEnumerable\<StockSharp.BusinessEntities.Order\>](xref:System.Collections.Generic.IEnumerable`1) newOrders **)**.
3. After the orders are loaded into the strategy, all trades executed on them will also be loaded. This will be done automatically.

The following example shows how to load all trades into a strategy:

## Loading Previously Executed Orders and Trades into a Strategy

1. For [Strategy](xref:StockSharp.Algo.Strategies.Strategy) to load its previous state, you need to override [Strategy.ProcessNewOrders](xref:StockSharp.Algo.Strategies.Strategy.ProcessNewOrders(System.Collections.Generic.IEnumerable{StockSharp.BusinessEntities.Order}))**(**[System.Collections.Generic.IEnumerable\<StockSharp.BusinessEntities.Order\>](xref:System.Collections.Generic.IEnumerable`1) newOrders **)**. This method will receive all orders from the connector during [Strategy.OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted), and you need to filter them:

```cs
   private bool _isOrdersLoaded;
   private bool _isStopOrdersLoaded;
   		  	
   protected override IEnumerable<Order> ProcessNewOrders(IEnumerable<Order> newOrders, bool isStopOrders)
   {
   	// if orders have already been loaded previously
   	if ((!isStopOrders && _isOrdersLoaded) || (isStopOrders && _isStopOrdersLoaded))
   		return base.ProcessNewOrders(newOrders, isStopOrders);
   	return Filter(newOrders);
   }
```

2. To implement order filtering, you need to define a screening criterion. For example, if during the strategy operation you save all registered orders to a file, you can create a filter by transaction number [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId). If such a number is present in the file, it means the order was registered through this strategy:

```cs
   private IEnumerable<Order> Filter(IEnumerable<Order> orders)
   {
   	// read transaction numbers from the file
   	var transactions = File.ReadAllLines("orders_{0}.txt".Put(Name)).Select(l => l.To<long>()).ToArray();
   	
   	// find our orders by the read numbers
   	return orders.Where(o => transactions.Contains(o.TransactionId));
   }
```

3. Recording transaction numbers of orders registered through the strategy can be done by overriding the [Strategy.RegisterOrder](xref:StockSharp.Algo.Strategies.Strategy.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** method:

```cs
   protected override void RegisterOrder(Order order)
   {
   	// send the order further for registration
   	base.RegisterOrder(order);
   	
   	// add a new transaction number
   	File.AppendAllLines("orders_{0}.txt".Put(Name), new[]{ order.TransactionId.ToString() });
   }
```

4. After the orders are loaded into the strategy, all trades executed on them will also be loaded. This will be done automatically.
