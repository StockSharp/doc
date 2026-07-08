# Komprimierung von Tick-Daten und Spreads zu Candles

## Einführung

Die API bietet leistungsstarke Werkzeuge zur Komprimierung von Tick-Daten und Spreads (beste Geld-/Briefkurse) zu Candles. Diese Funktionalität ist besonders nützlich für die Analyse historischer Daten oder die Erstellung benutzerdefinierter Indikatoren.

Die wichtigsten Erweiterungsmethoden für die Datenkomprimierung befinden sich in der Klasse `CandleHelper`. Der vollständige Quellcode dieser Klasse ist [auf GitHub verfügbar](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

Es wird empfohlen, diese Datei zu überprüfen, um ein vollständiges Verständnis aller verfügbaren Methoden und ihrer Parameter zu erhalten.

## Komprimierungsmethoden

### Komprimierung von Tick-Daten zu Candles

```cs
// Beispiel für die Verwendung von ToCandles für Ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// Dieser Code lädt Tickdaten aus dem Speicher und wandelt sie in Kerzen um.
// mdMsg - the message with parameters of the created candles (type, time frame, etc.).
// candleBuilderProvider — Provider, der eine konkrete Kerzen-Builder-Implementierung bereitstellt.
```

### Komprimierung von Spread-Daten zu Candles

```cs
// Beispiel für die Verwendung von ToCandles für Spread-Daten
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Hier laden wir Spread-Daten und wandeln sie in Kerzen um.
// Level1Fields.SpreadMiddle bedeutet, dass die Spread-Mitte zum Erstellen von Kerzen verwendet wird.
// Sie können auch Level1Fields.BestBid bzw. Level1Fields.BestAsk für den besten Bid- oder Ask-Preis verwenden.
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

// Diese Methode zeigt verschiedene Möglichkeiten zum Erstellen von Kerzen je nach Quelldatentyp.
// Es unterstützt das Erstellen aus Ticks, Orderlog, Spreads und anderen Quellen.
```

## Zusätzliche Funktionen

### Erstellung von Candles aus verschiedenen Quellen

Die API ermöglicht die Erstellung von Candles nicht nur aus Ticks und Spreads, sondern auch aus anderen Datenquellen:

```cs
// Beispiel für das Erstellen von Kerzen aus verschiedenen Quellen
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

// Dieser Code zeigt, wie Kerzen aus verschiedenen Datenquellen erstellt werden: Ticks, Orderlog, Spreads, Level1-Daten und sogar Kerzen kleinerer Timeframes.
```

## Fazit

Die Datenkomprimierungsmethoden in der API bieten flexible Werkzeuge für die Arbeit mit Marktdaten. Sie ermöglichen die effiziente Umwandlung von Tick-Daten und Spread-Daten in Candles verschiedener Typen und Zeitintervalle, was besonders nützlich für die Marktanalyse und die Entwicklung von Handelsstrategien ist.