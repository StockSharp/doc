# Reglas para libros de órdenes y operaciones

## Descripción general

`SimpleRulesStrategy` es una estrategia que demuestra distintas formas de crear y aplicar reglas en StockSharp. Se suscribe a operaciones y libros de órdenes, y luego establece varias reglas para procesar los datos recibidos.

## Componentes principales

```cs
// Componentes principales
public class SimpleRulesStrategy : Strategy
{
}
```

## Método OnStarted

Se llama cuando se inicia la estrategia:

- Crea suscripciones a operaciones y libros de órdenes
- Demuestra distintas formas de crear y aplicar reglas

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	//-----------------------Crear una regla. Método №1-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//-----------------------Crear una regla. Método №2-----------------------------------
	var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

	whenMarketDepthChanged.Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//----------------------Regla dentro de una regla-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

		//----------------------no es una regla Once-----------------------------------
		mdSub.WhenOrderBookReceived(this).Do((depth1) =>
		{
			LogInfo($"The rule WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
		}).Apply(this);
	}).Once().Apply(this);

	// Envío de solicitudes para suscribirse a datos de mercado.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Lógica

### Método #1: Creación de una regla

- Crea una regla que se activa cuando se recibe un libro de órdenes
- Registra el mejor bid y ask
- La regla se activa solo una vez (`Once()`)

### Método #2: Creación de una regla

- Demuestra una forma alternativa de crear una regla
- Funcionalmente idéntica al método #1

### Regla dentro de una regla

- Crea una regla que se activa cuando se recibe un libro de órdenes
- Dentro de esta regla, se crea otra regla
- La regla externa se activa una vez; la regla interna, cada vez que se recibe un libro de órdenes

## Características

- Demuestra distintas formas de crear y aplicar reglas en StockSharp
- Usa suscripción a operaciones y libros de órdenes
- Muestra un ejemplo de logging de información en una estrategia mediante el método `LogInfo`
- Ilustra el uso de `Once()` para limitar la activación de reglas
