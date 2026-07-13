# Carga de órdenes y operaciones

Al iniciar una estrategia, puede ser necesario cargar órdenes y operaciones ejecutadas previamente (por ejemplo, cuando un robot se reinició durante una sesión de negociación o cuando las órdenes y operaciones se trasladan durante la noche). Para ello, debe:

1. Cargar los IDs de transacción de órdenes guardadas anteriormente (por ejemplo, desde un archivo).
2. Suscribirse al evento `OrderReceived` para registrar nuevos IDs de transacción para sesiones futuras.
3. Sobrescribir el método `CanAttach` para asociar con la estrategia órdenes colocadas previamente.
4. Después de adjuntar las órdenes a la estrategia, todas las operaciones ejecutadas sobre ellas se cargarán automáticamente.

El siguiente ejemplo muestra cómo cargar todas las operaciones en una estrategia:

## Cargar órdenes y operaciones ejecutadas previamente en una estrategia

1. Cuando se inicia la estrategia, cargue los números de transacción guardados y suscríbase a `OrderReceived` para almacenar los nuevos:

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

2. Sobrescriba `CanAttach` para que la estrategia pueda reconocer sus órdenes después de un reinicio:

```cs
protected override bool CanAttach(Order order)
{
	return _transactions.Contains(order.TransactionId);
}
```

3. Después de cargar las órdenes en la estrategia, todas las operaciones ejecutadas sobre ellas también se cargarán automáticamente.
