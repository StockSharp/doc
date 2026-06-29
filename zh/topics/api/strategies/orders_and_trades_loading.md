# 加载订单和交易

在启动策略时，可能需要加载之前执行的订单和交易（例如，当机器人在交易会话中重新启动或订单和交易在隔夜期间被延续）。为此，您需要：

1. 加载之前保存的订单的交易ID（例如，从文件中）。
2. 订阅 `OrderReceived` 事件以记录未来会话的新交易 ID。
3. 重写 `CanAttach` 方法，将先前下的订单与策略关联。
4. 在订单附加到策略后，所有在这些订单上执行的交易将自动加载。

以下示例显示如何将所有交易加载到策略中：

## 将先前执行的订单和交易加载到策略中

1. 当策略开始时，加载已保存的交易编号并订阅 `OrderReceived` 以存储新的编号：

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

2. 覆盖 `CanAttach`，以便策略在重启后能够识别其订单：

```cs
protected override bool CanAttach(Order order)
{
	return _transactions.Contains(order.TransactionId);
}
```

3. 在订单被加载到策略中后，对它们执行的所有交易也将自动被加载。
