# Regla Until

## Descripción general

`SimpleRulesUntilStrategy` es una estrategia que demuestra el uso de una regla con condición de terminación (`Until`) en StockSharp. Se suscribe a operaciones y libros de órdenes, y luego establece una regla que se ejecuta hasta que se cumple una condición determinada.

## Componentes principales

```cs
// Componentes principales
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## Método OnStarted

Se llama cuando se inicia la estrategia:

- Crea suscripciones a ticks y libros de órdenes
- Crea una regla que se ejecuta cuando se reciben datos del libro de órdenes hasta que se cumple una condición determinada

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	var i = 0;
	mdSub.WhenOrderBookReceived(this).Do(depth =>
	{
		i++;
		LogInfo($"Regla WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
		LogInfo($"Regla WhenOrderBookReceived i={i}");
	})
	.Until(() => i >= 10)
	.Apply(this);

	// Envío de solicitudes para suscribirse a datos de mercado.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Lógica

- Al iniciarse, la estrategia crea suscripciones a ticks y libros de órdenes
- Se crea una regla que se activa cada vez que se reciben datos del libro de órdenes
- Cuando la regla se activa:
  - El contador `i` se incrementa
  - La información sobre los mejores precios bid y ask se agrega al registro
  - El valor actual del contador `i` se agrega al registro
- La regla se ejecuta hasta que el valor del contador `i` alcanza o supera 10
- Después de que se cumple la condición, la regla deja de funcionar automáticamente

## Características

- Demuestra el uso del método `Until()` para limitar la ejecución de reglas
- Usa suscripción a operaciones y libros de órdenes
- Muestra un ejemplo de registro de información sobre el libro de órdenes y el estado del contador mediante el método `LogInfo`
- Ilustra cómo limitar el número de ejecuciones de una regla en función de una condición específica
