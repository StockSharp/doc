# Loading Orders and Trades

When starting a strategy, it may be necessary to load previously executed orders and trades (for example, when a robot was restarted during a trading session or when orders and trades are carried over overnight). To do this, you need to:

1. Load the transaction IDs of orders saved earlier (for example, from a file).
2. Subscribe to the `OrderReceived` event to record new transaction IDs for future sessions.
3. Override the `CanAttach` method to associate previously placed orders with the strategy.
4. After the orders are attached to the strategy, all trades executed on them will be loaded automatically.

The following example shows how to load all trades into a strategy:

## Loading Previously Executed Orders and Trades into a Strategy

1. When the strategy starts, load saved transaction numbers and subscribe to `OrderReceived` to store new ones:

```cs
private HashSet<long> _transactions;

protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_transactions = File.Exists($"orders_{Name}.txt")
			? File.ReadAllLines($"orders_{Name}.txt").Select(l => l.To<long>()).ToHashSet()
			: new HashSet<long>();

	OrderReceived += order =>
	{
			File.AppendAllLines($"orders_{Name}.txt", new[] { order.TransactionId.ToString() });
			_transactions.Add(order.TransactionId);
	};
}
```

2. Override `CanAttach` so the strategy can recognize its orders after a restart:

```cs
protected override bool CanAttach(Order order)
{
	return _transactions.Contains(order.TransactionId);
}
```

3. After the orders are loaded into the strategy, all trades executed on them will also be loaded automatically.
