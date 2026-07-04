# Speicherformate

StockSharp unterstützt zwei Marktdatenspeicherformate, die durch die Enumeration [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) definiert werden: **Binary** und **CSV**. Jedes Format hat eigene Vorteile und eignet sich für unterschiedliche Anwendungsfälle.

## Binärformat

Das Binärformat ist das primäre Hochleistungsformat zur Speicherung von Daten in StockSharp. Die Implementierung befindet sich in der Klasse `BinaryMarketDataSerializer` innerhalb von `Algo/Storages/Binary/`.

### Eigenschaften

- **Kompaktheit** - Daten werden binär mit Komprimierung serialisiert, wodurch minimale Dateigrößen erreicht werden.
- **Performance** - Lesen und Schreiben sind deutlich schneller als bei Textformaten.
- **Metadaten** - die Klasse `BinaryMetaInfo` speichert Hilfsinformationen: erste und letzte Preise, Bruchteilswerte, Zeitstempel. Dadurch können allgemeine Dateninformationen schnell abgerufen werden, ohne die Datei vollständig zu lesen.
- **Kompressionsfähige Architektur** - das Format ist von Grund auf für effiziente Streaming-Komprimierung ausgelegt.

Binärdateien haben die Erweiterung `.bin`.

### Unterstützte Datentypen

Das Binärformat unterstützt die Serialisierung aller wichtigen Marktdatentypen: Candles, Ticks (Trades), Orderbücher (Level2), Level1-Daten, Orders und eigene Trades. Jeder Typ besitzt einen spezialisierten Serializer, der für die Struktur der jeweiligen Daten optimiert ist.

## CSV-Format

Das Textformat CSV (Comma-Separated Values) ist in der Klasse `CsvMarketDataSerializer` implementiert, die sich in `Algo/Storages/Csv/` befindet.

### Eigenschaften

- **Lesbarkeit** - Dateien können in jedem Texteditor oder in Excel zur visuellen Datenanalyse geöffnet werden.
- **Bearbeitbarkeit** - Daten können bei Bedarf manuell korrigiert werden.
- **Metadaten** - die Klasse `CsvMetaInfo` stellt Metadatenspeicherung mit Encoding-Unterstützung bereit. Sie enthält die Eigenschaft `IncrementalOnly` (Unterstützung inkrementeller Daten) und `LastId` (letzter Datensatzbezeichner).
- **Größere Dateigröße** - die Textdarstellung benötigt deutlich mehr Speicherplatz.
- **Langsamere Verarbeitung** - das Parsen von Textdaten erfordert zusätzliche Rechenressourcen.

CSV-Dateien haben die Erweiterung `.csv`.

## Wann welches Format verwendet werden sollte

| Szenario | Empfohlenes Format |
|----------|-------------------|
| Produktionseinsatz | Binary |
| Große Datenmengen | Binary |
| Performance-kritisch | Binary |
| Debugging und Diagnose | CSV |
| Visuelle Datenanalyse | CSV |
| Integration mit externen Tools | CSV |
| Manuelle Datenkorrektur | CSV |

## Dateiorganisation auf dem Datenträger

Daten werden auf dem Datenträger gemäß folgender Pfadstruktur organisiert:

```
{root_folder}/{first_letter}/{instrument_identifier}/{yyyy_MM_dd}/{file_name}.{extension}
```

Die Erweiterung ist `.bin` für das Binärformat oder `.csv` für das Textformat. Diese hierarchische Organisation ermöglicht eine schnelle Datensuche nach Instrument und Datum.

Zum Beispiel würden 5-Minuten-Candles für das Instrument AAPL@NASDAQ am 1. April 2024 im Binärformat unter einem Pfad wie diesem liegen:

```
Storage/S/AAPL@NASDAQ/2024_04_01/candles_5m.bin
```

## Konvertierung zwischen Formaten

StockSharp ermöglicht es, Daten in einem Format zu laden und in einem anderen zu speichern. Das kann beispielsweise nützlich sein, um Binärdaten als CSV für die Analyse in externen Tools zu exportieren:

```cs
// Aus Binärspeicher laden
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Binary);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();

// In CSV-Speicher speichern
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Csv);
await csvStorage.SaveAsync(candles);
```

## Codebeispiel

Das Format wird beim Erstellen eines Speichers über [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) ausgewählt:

```cs
var storageRegistry = new StorageRegistry();

// Candle-Speicher im Binärformat erstellen
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Binary);

// Candle-Speicher im CSV-Format erstellen
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Csv);

// Daten laden
var from = new DateTime(2024, 1, 1);
var to = new DateTime(2024, 1, 31);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();
```

Das Speicherformat kann auch bei der Arbeit mit Tick-Daten und Orderbüchern angegeben werden:

```cs
// Ticks im Binärformat
var tickStorage = storageRegistry.GetTickMessageStorage(
    securityId, StorageFormats.Binary);

// Orderbücher im CSV-Format
var depthStorage = storageRegistry.GetQuoteMessageStorage(
    securityId, StorageFormats.Csv);
```

## Siehe auch

- [Working with the API](api.md)
- [Storage Drives](drives.md)

