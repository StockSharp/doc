# Snapshot-System

Snapshots in StockSharp stellen einen Mechanismus zum Speichern des letzten aktuellen Zustands von Marktdaten dar. Statt die gesamte Historie zu scannen, ermöglichen Snapshots den sofortigen Abruf des aktuellen Level1-Werts, Orderbuchs, der Position oder Transaktion.

## Zweck von Snapshots

Bei der Arbeit mit Datenströmen ist es häufig notwendig, den letzten Zustand eines Instruments zu kennen - aktuellen Preis, Orderbuch, offene Position. Ohne Snapshots müsste dafür die gesamte Historie geladen und verarbeitet werden. Das Snapshot-System löst dieses Problem, indem es den letzten Zustand jedes Objekts speichert und Zugriff darauf in minimaler Zeit ermöglicht.

## ISnapshotStorage - Interface für Snapshot-Speicher

Das Interface [ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) definiert den Basiskontrakt für die Arbeit mit Snapshots. Die typisierte Version `ISnapshotStorage<TKey, TMessage>` stellt folgende Methoden bereit:

- **Update(message)** - Snapshot speichern oder aktualisieren. Wenn für den angegebenen Schlüssel bereits ein Snapshot existiert, wird er aktualisiert.
- **Get(key)** - Snapshot nach Schlüssel abrufen (z. B. nach Instrumentenbezeichner).
- **GetAll(from, to)** - alle Snapshots für den angegebenen Datumsbereich abrufen.
- **Clear(key)** - Snapshot für einen bestimmten Schlüssel löschen.
- **ClearAll()** - alle Snapshots löschen.

## ISnapshotSerializer - Snapshot-Serialisierung

Das Interface [ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) ist für die Konvertierung von Snapshots in eine Binärdarstellung und zurück verantwortlich:

- **DataType** - Informationen zum Snapshot-Datentyp.
- **Version** - Version des Serialisierungsformats.
- **Serialize(version, message)** - Nachricht in ein Bytearray serialisieren.
- **Deserialize(version, buffer)** - Bytearray zurück in eine Nachricht deserialisieren.
- **GetKey(message)** - Schlüssel aus einer Nachricht extrahieren.
- **Update(message, changes)** - inkrementelle Änderungen auf einen bestehenden Snapshot anwenden.

## SnapshotRegistry - Snapshot-Registry

Die Klasse [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) ist die zentrale Komponente für die Snapshot-Verwaltung. Sie implementiert das Interface `ISnapshotRegistry` und koordiniert den Betrieb aller Snapshot-Speicher.

### Wichtige Eigenschaften

- **Speicherverwaltung** - bietet Zugriff auf Snapshot-Speicher für verschiedene Datentypen.
- **Periodisches Schreiben** - Änderungen werden alle 10 Sekunden auf den Datenträger geschrieben, wodurch ein Gleichgewicht zwischen Performance und Zuverlässigkeit erreicht wird.
- **Threadsicherheit** - alle Operationen sind sicher für die Verwendung aus mehreren Threads.

### Dateiorganisation

Snapshot-Dateien werden unter folgendem Pfad gespeichert:

```
{path}/{yyyy_MM_dd}/{serializer_name}.bin
```

## Integrierte Serializer

StockSharp enthält vier Serializer für die wichtigsten Marktdatentypen:

| Serializer | Message Type | Zweck |
|------------|-------------|---------|
| [Level1BinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.Level1BinarySnapshotSerializer) | `Level1ChangeMessage` | Level1-Daten (Preise, Volumina, Spreads) |
| [QuotesBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.QuotesBinarySnapshotSerializer) | `QuoteChangeMessage` | Orderbuch |
| [PositionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.PositionBinarySnapshotSerializer) | `PositionChangeMessage` | Positionen |
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | Transaktionen (Orders und Trades) |

Jeder Serializer unterstützt Formatversionierung und gewährleistet damit Rückwärtskompatibilität beim Aktualisieren von StockSharp.

## Codebeispiel

### Snapshot Registry erstellen

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### Arbeiten mit Level1-Snapshots

```cs
// Level1-Snapshot-Speicher abrufen
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// Snapshot speichern
var level1Msg = new Level1ChangeMessage
{
    SecurityId = "AAPL@NASDAQ".ToSecurityId(),
    ServerTime = DateTimeOffset.Now,
};
level1Msg.TryAdd(Level1Fields.LastTradePrice, 260.5m);
level1Msg.TryAdd(Level1Fields.BestBidPrice, 260.4m);
level1Msg.TryAdd(Level1Fields.BestAskPrice, 260.6m);

level1Snapshots.Update(level1Msg);
```

### Snapshot abrufen

```cs
// Letzten Snapshot für ein Instrument abrufen
var secId = "AAPL@NASDAQ".ToSecurityId();
var snapshot = level1Snapshots.Get(secId);

if (snapshot != null)
{
    Console.WriteLine($"Letzter Preis: {snapshot.Changes[Level1Fields.LastTradePrice]}");
}
```

## Siehe auch

- [Arbeiten mit der API](api.md)
- [Speicherformate](formats.md)
- [Speicherlaufwerke](drives.md)

