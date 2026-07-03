# Reglas para operaciones tick

## Descripción general

`SimpleTradeRulesStrategy` es una estrategia que demuestra el uso de reglas combinadas para analizar precios de operaciones en StockSharp. Se suscribe a operaciones y crea una regla que se activa bajo determinadas condiciones de precio.

## Componentes principales

```cs
// Componentes principales
public class SimpleTradeRulesStrategy : Strategy
{
}
```

## Método OnStarted

Se llama cuando se inicia la estrategia:

- Crea una suscripción a ticks
- Crea una regla combinada para analizar precios de operaciones

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(t =>
	{
		sub
			.WhenLastTradePriceMore(this, t.Price + 2)
			.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
			.Do(t =>
			{
				LogInfo($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess tick={t}");
			})
			.Apply(this);
	})
	.Once() // llamar esta regla solo una vez
	.Apply(this);

	// Envío de solicitud para suscribirse a datos de mercado.
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## Lógica

- Cuando se recibe el primer tick, se crea una regla combinada
- Se basa en el precio del tick recibido: crea una regla que se activa cuando el precio cambia en +/- 2
- La regla se activa cuando el precio de la última operación supera el actual + 2 o queda por debajo del actual - 2
- Cuando la regla se activa, se agrega al log información sobre el tick
- La regla externa se activa solo una vez (`Once()`)

## Características

- Demuestra la creación de reglas combinadas mediante `Or()`
- Usa `WhenLastTradePriceMore` y `WhenLastTradePriceLess` para el análisis de precios
- Muestra un ejemplo de logging de información sobre operaciones mediante el método `LogInfo`
- Ilustra el uso de `Once()` para limitar la activación de reglas
- Pasa el parámetro tick al controlador de eventos (a diferencia del ejemplo de la documentación)
