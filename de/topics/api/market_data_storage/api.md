# Arbeiten mit der API

## Vorbereitung

Für die Arbeit mit historischen Daten in den Beispielen wird ein NuGet-Paket mit Beispieldaten verwendet. Es kann aus der [NuGet Gallery](https://www.nuget.org/packages/StockSharp.Samples.HistoryData) installiert werden. Dieses Paket stellt einen Datensatz bereit, mit dem die Arbeit mit dem Speicher demonstriert werden kann.

Der gesamte Code ist im [StockSharp repository](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage) verfügbar.

## Erstellen einer Storage Registry

Für die Arbeit mit Marktdatenspeicher in StockSharp wird die Klasse [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) verwendet. Beim Erstellen eines Objekts dieser Klasse können Sie den Pfad zum Standardspeicher über die Eigenschaft [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) festlegen oder mit [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) einen bestimmten Ordner für historische Daten angeben.

```cs
// StorageRegistry mit Standardpfad erstellen
var storageRegistry = new StorageRegistry();
```

```cs
// StorageRegistry mit dem Pfad zu Daten aus dem NuGet-Paket erstellen
var pathHistory = Paths.HistoryDataPath; // Pfad zu Daten aus dem NuGet-Paket
var localDrive = new LocalMarketDataDrive(pathHistory);
var storageRegistry = new StorageRegistry()
{
	DefaultDrive = localDrive,
};
```

## Daten abrufen

Über [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) können Sie für den gewünschten Zeitraum auf verschiedene Typen von Marktdaten zugreifen. Dafür werden folgende Methoden verwendet:

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageHelper.GetTimeFrameCandleMessageStorage(StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) für Candles
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) für Ticks
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) für Orderbücher

Jede dieser Methoden gibt den entsprechenden Speicher zurück, aus dem Daten mit der Methode `LoadAsync` geladen werden können, wobei Start- und Enddatum angegeben werden.

```cs
// Candles abrufen
var securityId = "AAPL@NASDAQ".ToSecurityId();
var candleStorage = storageRegistry.GetTimeFrameCandleMessageStorage(securityId, TimeSpan.FromMinutes(1), StorageFormats.Binary);
var candles = candleStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var candle in candles)
{
	Console.WriteLine(candle);
}
```

```cs
// Ticks abrufen
var tradeStorage = storageRegistry.GetTickMessageStorage(securityId, StorageFormats.Binary);
var trades = tradeStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var trade in trades)
{
	Console.WriteLine(trade);
}
```

```cs
// Orderbücher abrufen
var marketDepthStorage = storageRegistry.GetQuoteMessageStorage(securityId, StorageFormats.Binary);
var marketDepths = marketDepthStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var marketDepth in marketDepths)
{
	Console.WriteLine(marketDepth);
}
```

## Daten speichern

Um neue Daten in einen bestehenden Speicher zu schreiben, verwenden Sie die Methode `SaveAsync` des entsprechenden Speichers. Dadurch können historische Daten um neue Werte ergänzt werden.

```cs
// Neue Candles speichern
var newCandles = new List<CandleMessage>
{
	// Hier werden neue CandleMessage-Objekte erstellt
};
await candleStorage.SaveAsync(newCandles);
```

```cs
// Neue Ticks speichern
var newTrades = new List<ExecutionMessage>
{
	// Hier werden neue ExecutionMessage-Objekte für Ticks erstellt
};
await tradeStorage.SaveAsync(newTrades);
```

```cs
// Neue Orderbücher speichern
var newMarketDepths = new List<QuoteChangeMessage>
{
	// Hier werden neue QuoteChangeMessage-Objekte für Orderbücher erstellt
};
await marketDepthStorage.SaveAsync(newMarketDepths);
```

## Daten löschen

Um Daten für einen bestimmten Zeitraum zu löschen, verwenden Sie die Methode `DeleteAsync` des entsprechenden Speichers. Seien Sie beim Löschen von Daten aus dem Beispielpaket vorsichtig.

```cs
// Candles für den angegebenen Zeitraum löschen
await candleStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Ticks für den angegebenen Zeitraum löschen
await tradeStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Orderbücher für den angegebenen Zeitraum löschen
await marketDepthStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

Diese Operationen ermöglichen eine effiziente Verwaltung historischer Daten, unabhängig davon, ob sie über [Hydra](../../hydra.md) geladen, im NuGet-Paket bereitgestellt oder während der Ausführung Ihrer Anwendung erstellt wurden.

