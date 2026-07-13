# Komprimierung von Tick-Daten und Spreads zu Kerzen

## Einführung

Die API bietet leistungsstarke Werkzeuge zur Komprimierung von Tick-Daten und Spreads (beste Geld-/Briefkurse) zu Kerzen. Diese Funktionalität ist besonders nützlich für die Analyse historischer Daten oder die Erstellung benutzerdefinierter Indikatoren.

Die wichtigsten Erweiterungsmethoden für die Datenkomprimierung befinden sich in der Klasse `CandleHelper`. Der vollständige Quellcode dieser Klasse ist [auf GitHub verfügbar](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

Es wird empfohlen, diese Datei zu überprüfen, um ein vollständiges Verständnis aller verfügbaren Methoden und ihrer Parameter zu erhalten.

## Komprimierungsmethoden

### Komprimierung von Tick-Daten zu Kerzen

```cs
// Beispiel für die Verwendung von ToCandles für Ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// Dieser Code lädt Tickdaten aus dem Speicher und wandelt sie in Kerzen um.
// mdMsg - Nachricht mit Parametern der zu erstellenden Kerzen (Typ, Zeitrahmen usw.).
// candleBuilderProvider — Provider, der eine konkrete Kerzen-Builder-Implementierung bereitstellt.
```

### Komprimierung von Spread-Daten zu Kerzen

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

- `series`: Die Kerzenserie, die den Typ und die Parameter der erstellten Kerzen definiert.
- `type`: Der Datentyp für die Bildung von Kerzen (z. B. bester Geldkurs, bester Briefkurs oder Spread-Mitte).
- `candleBuilderProvider`: Der Provider für den Kerzen-Builder (optionaler Parameter).

## Anwendungsbeispiel

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ... (Initialisierungscode)

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ... (Code zum Erstellen von Kerzen aus dem Orderprotokoll)

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (andere Fälle)
	}
}

// Diese Methode zeigt verschiedene Möglichkeiten zum Erstellen von Kerzen je nach Quelldatentyp.
// Es unterstützt das Erstellen aus Ticks, Orderprotokoll, Spreads und anderen Quellen.
```

## Zusätzliche Funktionen

### Erstellung von Kerzen aus verschiedenen Quellen

Die API ermöglicht die Erstellung von Kerzen nicht nur aus Ticks und Spreads, sondern auch aus anderen Datenquellen:

```cs
// Beispiel für das Erstellen von Kerzen aus verschiedenen Quellen
switch (type)
{
	case BuildTypes.Ticks:
		// ... (Code für Ticks)

	case BuildTypes.OrderLog:
		// ... (Code für Orderprotokoll)

	case BuildTypes.Depths:
		// ... (Code für Spreads)

	case BuildTypes.Level1:
		// ... (Code für Level1)

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.LoadAsync(from, to);

	// ... (andere Fälle)
}

// Dieser Code zeigt, wie Kerzen aus verschiedenen Datenquellen erstellt werden: Ticks, Orderprotokoll, Spreads, Level1-Daten und sogar Kerzen kleinerer Zeitrahmen.
```

## Fazit

Die Datenkomprimierungsmethoden in der API bieten flexible Werkzeuge für die Arbeit mit Marktdaten. Sie ermöglichen die effiziente Umwandlung von Tick-Daten und Spread-Daten in Kerzen verschiedener Typen und Zeitintervalle, was besonders nützlich für die Marktanalyse und die Entwicklung von Handelsstrategien ist.
