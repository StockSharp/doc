# Orders und Trades laden

Beim Start einer Strategie kann es erforderlich sein, zuvor ausgeführte Orders und Trades zu laden (zum Beispiel, wenn ein Roboter während einer Handelssitzung neu gestartet wurde oder wenn Orders und Trades über Nacht übernommen werden). Dazu müssen Sie:

1. Die Transaktions-IDs zuvor gespeicherter Orders laden (zum Beispiel aus einer Datei).
2. Das Ereignis `OrderReceived` abonnieren, um neue Transaktions-IDs für zukünftige Sitzungen aufzuzeichnen.
3. Die Methode `CanAttach` überschreiben, um zuvor platzierte Orders der Strategie zuzuordnen.
4. Nachdem die Orders an die Strategie angehängt wurden, werden alle darauf ausgeführten Trades automatisch geladen.

Das folgende Beispiel zeigt, wie alle Trades in eine Strategie geladen werden:

## Zuvor ausgeführte Orders und Trades in eine Strategie laden

1. Laden Sie beim Start der Strategie gespeicherte Transaktionsnummern und abonnieren Sie `OrderReceived`, um neue zu speichern:

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

2. Überschreiben Sie `CanAttach`, damit die Strategie ihre Orders nach einem Neustart erkennen kann:

```cs
protected override bool CanAttach(Order order)
{
	return _transactions.Contains(order.TransactionId);
}
```

3. Nachdem die Orders in die Strategie geladen wurden, werden auch alle auf ihnen ausgeführten Trades automatisch geladen.
