# Komprimierung von Tick-Daten und Spreads zu Candles

## Einführung

Die API bietet leistungsstarke Werkzeuge zur Komprimierung von Tick-Daten und Spreads (beste Geld-/Briefkurse) zu Candles. Diese Funktionalität ist besonders nützlich für die Analyse historischer Daten oder die Erstellung benutzerdefinierter Indikatoren.

Die wichtigsten Erweiterungsmethoden für die Datenkomprimierung befinden sich in der Klasse `CandleHelper`. Der vollständige Quellcode dieser Klasse ist [auf GitHub verfügbar](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

Es wird empfohlen, diese Datei zu überprüfen, um ein vollständiges Verständnis aller verfügbaren Methoden und ihrer Parameter zu erhalten.

## Komprimierungsmethoden

### Komprimierung von Tick-Daten zu Candles

```cs
// Example usage of ToCandles for ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// This code loads tick data from storage and converts it into candles.
// mdMsg - the message with parameters of the created candles (type, time frame, etc.).
// candleBuilderProvider - the provider that supplies a specific candle builder implementation.
```

### Komprimierung von Spread-Daten zu Candles

```cs
// Example usage of ToCandles for spread data
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Here we load spread data and convert it into candles.
// Level1Fields.SpreadMiddle indicates using the spread middle price for building candles.
// You can also use Level1Fields.BestBid or Level1Fields.BestAsk for the best bid or ask prices, respectively.
```

## Komprimierungsparameter

Bei der Komprimierung von Daten können folgende Parameter angegeben werden:

- `series`: Die Candle-Serie, die den Typ und die Parameter der erstellten Candles definiert.
- `type`: Der Datentyp für die Bildung von Candles (z. B. bester Geldkurs, bester Briefkurs oder Spread-Mitte).
- `candleBuilderProvider`: Der Provider für den Candle-Builder (optionaler Parameter).

## Anwendungsbeispiel

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

## Zusätzliche Funktionen

### Erstellung von Candles aus verschiedenen Quellen

Die API ermöglicht die Erstellung von Candles nicht nur aus Ticks und Spreads, sondern auch aus anderen Datenquellen:

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

## Fazit

Die Datenkomprimierungsmethoden in der API bieten flexible Werkzeuge für die Arbeit mit Marktdaten. Sie ermöglichen die effiziente Umwandlung von Tick-Daten und Spread-Daten in Candles verschiedener Typen und Zeitintervalle, was besonders nützlich für die Marktanalyse und die Entwicklung von Handelsstrategien ist.