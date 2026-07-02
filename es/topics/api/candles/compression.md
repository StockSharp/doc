# Compresión de datos de ticks y spreads en velas

## Introducción

La API proporciona potentes herramientas para comprimir datos de ticks y spreads (precios de la mejor oferta/demanda) en velas. Esta funcionalidad es especialmente útil para analizar datos históricos o construir indicadores personalizados.

Los principales métodos de extensión para la compresión de datos se encuentran en la clase `CandleHelper`. El código fuente completo de esta clase está [disponible en GitHub](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

Se recomienda revisar este archivo para obtener una comprensión completa de todos los métodos disponibles y sus parámetros.

## Métodos de compresión

### Compresión de datos de ticks en velas

```cs
// Ejemplo de uso de ToCandles para ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// This code loads tick data from storage and converts it into candles.
// mdMsg - the message with parameters of the created candles (type, time frame, etc.).
// candleBuilderProvider - the provider that supplies a specific candle builder implementation.
```

### Compresión de datos de spread en velas

```cs
// Ejemplo de uso de ToCandles para datos de spread
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Here we load spread data and convert it into candles.
// Level1Fields.SpreadMiddle indicates using the spread middle price for building candles.
// You can also use Level1Fields.BestBid or Level1Fields.BestAsk for the best bid or ask prices, respectively.
```

## Parámetros de compresión

Al comprimir datos, se pueden especificar los siguientes parámetros:

- `series`: La serie de velas que define el tipo y los parámetros de las velas creadas.
- `type`: El tipo de datos para formar las velas (por ejemplo, mejor oferta, mejor demanda o punto medio del spread).
- `candleBuilderProvider`: El proveedor del constructor de velas (parámetro opcional).

## Ejemplo de uso

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ... (initialization code)

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ... (code for building candles from order log)

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (other cases)
	}
}

// This method demonstrates various ways to build candles depending on the type of source data.
// It supports building from ticks, order log, spreads, and other sources.
```

## Características adicionales

### Construcción de velas a partir de diversas fuentes

La API permite construir velas no solo a partir de ticks y spreads, sino también de otras fuentes de datos:

```cs
// Example of building candles from various sources
switch (type)
{
	case BuildTypes.Ticks:
		// ... (code for ticks)

	case BuildTypes.OrderLog:
		// ... (code for order log)

	case BuildTypes.Depths:
		// ... (code for spreads)

	case BuildTypes.Level1:
		// ... (code for Level1)

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.LoadAsync(from, to);

	// ... (other cases)
}

// This code shows how to build candles from different data sources: ticks, order log, spreads, Level1 data, and even from smaller time frame candles.
```

## Conclusión

Los métodos de compresión de datos en la API proporcionan herramientas flexibles para trabajar con datos de mercado. Permiten la conversión eficiente de datos de ticks y datos de spread en velas de varios tipos e intervalos de tiempo, lo cual es particularmente útil para el análisis de mercado y el desarrollo de estrategias de trading.
