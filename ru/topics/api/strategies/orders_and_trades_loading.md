# Загрузка заявок и сделок

При старте стратегии может возникнуть необходимость загрузки ранее совершённых заявок и сделок (например, когда робот был перезагружен в течение торговой сессии или сделки и заявки переносятся через ночь). Для этого нужно: 

1. Загрузить номера транзакций заявок, которые ранее сохранила стратегия (например, из файла).
2. Подписаться на событие `OrderReceived` и сохранять новые номера транзакций для последующих запусков.
3. Переопределить метод `CanAttach`, чтобы после перезапуска стратегия смогла распознать свои заявки.
4. После присоединения заявок к стратегии все совершённые по ним сделки загрузятся автоматически.

Следующий пример показывает загрузку всех сделок в стратегию: 

## Загрузка в стратегию ранее совершенных заявок и сделок

1. При старте стратегии загрузите сохранённые номера транзакций и подпишитесь на `OrderReceived`, чтобы сохранять новые номера:

```cs
private HashSet<long> _transactions;

protected override void OnStarted(DateTimeOffset time)
{
		base.OnStarted(time);

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

2. Переопределите `CanAttach`, чтобы после перезапуска стратегия могла определить свои заявки:

```cs
protected override bool CanAttach(Order order)
{
		return _transactions.Contains(order.TransactionId);
}
```

3. После того, как заявки будут загружены в стратегию, загрузятся и все совершённые по ним сделки автоматически.
