# Carregamento de Ordens e Negócios

Ao iniciar uma estratégia, pode ser necessário carregar ordens e negócios executados anteriormente (por exemplo, quando um robô foi reiniciado durante uma sessão de negociação ou quando ordens e negócios são transportados de um dia para o outro). Para isso, é necessário:

1. Carregar os IDs de transação das ordens guardadas anteriormente (por exemplo, a partir de um ficheiro).
2. Subscrever o evento `OrderReceived` para registar novos IDs de transação para sessões futuras.
3. Substituir o método `CanAttach` para associar à estratégia as ordens colocadas anteriormente.
4. Depois de as ordens serem anexadas à estratégia, todos os negócios executados sobre elas serão carregados automaticamente.

O exemplo seguinte mostra como carregar todos os negócios para uma estratégia:

## Carregar Ordens e Negócios Executados Anteriormente numa Estratégia

1. Quando a estratégia inicia, carregue os números de transação guardados e subscreva `OrderReceived` para armazenar os novos:

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

2. Substitua `CanAttach` para que a estratégia consiga reconhecer as suas ordens após um reinício:

```cs
protected override bool CanAttach(Order order)
{
	return _transactions.Contains(order.TransactionId);
}
```

3. Depois de as ordens serem carregadas para a estratégia, todos os negócios executados sobre elas também serão carregados automaticamente.
