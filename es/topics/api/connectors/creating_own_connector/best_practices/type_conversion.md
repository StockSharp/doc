# Conversión de Tipos

El componente de conversión de tipos desempeña un papel importante para garantizar la compatibilidad entre los tipos de datos utilizados en StockSharp y los formatos específicos de un exchange en particular.

## Funciones Principales

1. Conversión de los tipos de StockSharp (por ejemplo, [Sides](xref:StockSharp.Messages.Sides), [OrderTypes](xref:StockSharp.Messages.OrderTypes), [TimeInForce](xref:StockSharp.Messages.TimeInForce)) a las representaciones de cadena utilizadas por el exchange.
2. Conversión inversa de los datos recibidos del exchange a los tipos de StockSharp.
3. Conversión de identificadores de instrumentos entre los formatos de StockSharp y del exchange.
4. Conversión de formatos de hora y marcos temporales.

## Ejemplo de Implementación

A continuación se muestra un ejemplo de una clase con métodos de extensión para la conversión de tipos:

```cs
static class Extensions
{
	// Convertir el lado de la orden de StockSharp a la representación de cadena del exchange
	public static string ToNative(this Sides side)
	{
		return side switch
		{
			Sides.Buy => "buy",
			Sides.Sell => "sell",
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};
	}

	// Convertir la representación de cadena del lado de la orden del exchange al tipo de StockSharp
	public static Sides ToSide(this string side)
		=> side?.ToLowerInvariant() switch
		{
			"buy" or "bid" => Sides.Buy,
			"sell" or "ask" or "offer" => Sides.Sell,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};

	// Convertir el tipo de orden de StockSharp a la representación de cadena del exchange
	public static string ToNative(this OrderTypes? type)
	{
		return type switch
		{
			null => null,
			OrderTypes.Limit => "limit",
			OrderTypes.Market => "market",
			OrderTypes.Conditional => "stop",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};
	}

	// Convertir la representación de cadena del tipo de orden del exchange al tipo de StockSharp
	public static OrderTypes ToOrderType(this string type)
		=> type?.ToLowerInvariant() switch
		{
			"limit" => OrderTypes.Limit,
			"market" => OrderTypes.Market,
			"stop" or "stop limit" => OrderTypes.Conditional,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};

	// Otros métodos de conversión...

	// Diccionario para mapear marcos temporales de StockSharp a representaciones de cadena de la bolsa
	public static readonly PairSet<TimeSpan, string> TimeFrames = new()
	{
		{ TimeSpan.FromMinutes(1), "ONE_MINUTE" },
		{ TimeSpan.FromMinutes(5), "FIVE_MINUTE" },
		// Otros marcos temporales...
	};

	// Convertir el marco temporal de StockSharp a la representación de cadena de la bolsa
	public static string ToNative(this TimeSpan timeFrame)
		=> TimeFrames.TryGetValue(timeFrame) ?? throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, LocalizedStrings.InvalidValue);

	// Convertir la representación de cadena del marco temporal de la bolsa a TimeSpan
	public static TimeSpan ToTimeFrame(this string name)
		=> TimeFrames.TryGetKey2(name) ?? throw new ArgumentOutOfRangeException(nameof(name), name, LocalizedStrings.InvalidValue);
}
```

## Recomendaciones

- Utilice métodos de extensión para un uso cómodo de las funciones de conversión.
- Maneje todos los valores de enumeración posibles, incluidos `null` y valores desconocidos.
- Utilice expresiones `switch` (C# 8.0+) para un código más limpio y legible.
- Añada comprobaciones de valores no válidos y lance excepciones con mensajes de error claros.
- Considere utilizar diccionarios para el mapeo de valores, especialmente para mapeos complejos o que cambian con frecuencia (por ejemplo, para los marcos temporales).

Una implementación adecuada de la conversión de tipos simplifica significativamente el trabajo con datos en otras partes del conector y reduce la probabilidad de errores relacionados con discrepancias de formato.
</content>
