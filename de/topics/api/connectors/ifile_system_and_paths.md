# IFileSystem und Paths.FileSystem

## Überblick

`IFileSystem` ist eine Dateisystemabstraktion, die in StockSharp durchgängig verwendet wird. Statt direkt auf `System.IO.File` und `System.IO.Directory` zuzugreifen, arbeiten Plattformkomponenten über diese Schnittstelle. Das bietet:

- **Testbarkeit** - die Möglichkeit, das Dateisystem in Unit-Tests zu ersetzen
- **Portabilität** - eine einheitliche Schnittstelle für verschiedene Umgebungen (lokaler Datenträger, Cloud-Speicher, Arbeitsspeicher)
- **Konsistenz** - alle Komponenten verwenden denselben Ansatz für die Arbeit mit Dateien

Die Schnittstelle `IFileSystem` ist im Paket `Ecng.Common` definiert und stellt Methoden für die Arbeit mit Dateien und Verzeichnissen bereit: Erstellen, Lesen, Schreiben, Löschen, Prüfen auf Existenz und Auflisten von Dateien.

## Paths.FileSystem

Die Klasse `Paths` (Namespace `StockSharp.Configuration`) stellt die statische Eigenschaft `FileSystem` bereit - die standardmäßige `IFileSystem`-Implementierung:

```csharp
public static class Paths
{
    // Standard-Dateisystem (LocalFileSystem)
    public static readonly IFileSystem FileSystem = Messages.Extensions.DefaultFileSystem;
}
```

`Paths.FileSystem` verweist auf `LocalFileSystem.Instance` - ein Singleton, das über die Standardfunktionen von `System.IO` mit dem lokalen Datenträger arbeitet.

## Wichtige IFileSystem-Methoden

Die Schnittstelle `IFileSystem` enthält Methoden für typische Operationen:

| Methode | Beschreibung |
|--------|-------------|
| `FileExists(path)` | Prüfen, ob eine Datei vorhanden ist |
| `DirectoryExists(path)` | Prüfen, ob ein Verzeichnis vorhanden ist |
| `CreateDirectory(path)` | Ein Verzeichnis erstellen |
| `OpenRead(path)` | Eine Datei zum Lesen öffnen (`Stream`) |
| `OpenWrite(path)` | Eine Datei zum Schreiben öffnen (`Stream`) |
| `MoveFile(src, dst)` | Eine Datei verschieben |
| `DeleteFile(path)` | Eine Datei löschen |
| `EnumerateFiles(path, mask)` | Dateien in einem Verzeichnis aufzählen |
| `WriteAllTextAsync(path, text)` | Text asynchron in eine Datei schreiben |

## Wo es verwendet wird

Praktisch alle StockSharp-Komponenten, die mit dem Dateisystem arbeiten, akzeptieren `IFileSystem` in ihrem Konstruktor. Unten sind die häufigsten Fälle aufgeführt.

### LocalMarketDataDrive

Marktdatenspeicher auf lokalem Datenträger:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Empfohlene Variante - IFileSystem explizit übergeben
var drive = new LocalMarketDataDrive(Paths.FileSystem, @"C:\MarketData");

// Veraltete Variante (verwendet Paths.FileSystem intern)
// var drive = new LocalMarketDataDrive(@"C:\MarketData"); // [Obsolete]
```

### CsvEntityRegistry

Entitätsregistrierung (Börsen, Instrumente, Portfolios) im CSV-Format:

```csharp
using StockSharp.Algo.Storages.Csv;
using StockSharp.Configuration;

var executor = new ChannelExecutor();
var registry = new CsvEntityRegistry(Paths.FileSystem, @"C:\Data", executor);
```

### SnapshotRegistry

Registrierung für Marktdaten-Snapshots:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

var snapshots = new SnapshotRegistry(Paths.FileSystem, Paths.SnapshotsDir);
```

### Serialisierung und Deserialisierung

Erweiterungsmethoden in `Paths` für die Arbeit mit JSON-Konfigurationen:

```csharp
using StockSharp.Configuration;

var fs = Paths.FileSystem;

// Objekt in eine Datei serialisieren
settings.Serialize(fs, @"C:\config.json");

// Objekt aus einer Datei deserialisieren
var loaded = @"C:\config.json".Deserialize<SettingsStorage>(fs);

// Asynchrone Deserialisierung
var data = await @"C:\data.json".DeserializeAsync<MyData>(fs, cancellationToken);

// Prüfen, ob eine Konfigurationsdatei vorhanden ist
if (@"C:\config.json".IsConfigExists(fs))
{
    // ...
}
```

### CandlePatternFileStorage

Speicher für Kerzenmuster:

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

var patternStorage = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);
```

### NativeIdStorage, SecurityMappingStorage und weitere

Die meisten Speicher für Zuordnungen und Kennungen akzeptieren ebenfalls `IFileSystem`:

```csharp
// NativeIdStorage
var nativeIdStorage = new NativeIdStorage(Paths.FileSystem, Paths.SecurityNativeIdDir, executor);

// SecurityMappingStorage
var mappingStorage = new SecurityMappingStorage(Paths.FileSystem, Paths.SecurityMappingDir, executor);

// ExtendedInfoStorage
var extInfoStorage = new ExtendedInfoStorage(Paths.FileSystem, Paths.SecurityExtendedInfo, executor);
```

## Typisches Verwendungsmuster

In StockSharp-Anwendungen wird empfohlen, eine Referenz auf `IFileSystem` zu halten und sie an alle Komponenten zu übergeben:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Dateisystem abrufen
var fs = Paths.FileSystem;

// Datenspeicher erstellen
var drive = new LocalMarketDataDrive(fs, Paths.StorageDir);

// Connector erstellen und Speicher konfigurieren
var connector = new Connector();
connector.Adapter.StorageSettings.Drive = drive;

// Einstellungen aus einer Datei laden
var configFile = Path.Combine(Paths.AppDataPath, "connector_config.json");

if (configFile.IsConfigExists(fs))
{
    var config = configFile.Deserialize<SettingsStorage>(fs);
    connector.Load(config);
}
```

## Veraltete Überladungen

Viele Klassen behalten aus Gründen der Abwärtskompatibilität Konstruktoren ohne `IFileSystem` bei, diese sind jedoch mit dem Attribut `[Obsolete]` markiert. Diese Konstruktoren verwenden intern `Paths.FileSystem`:

```csharp
// Veraltete Variante
[Obsolete("IFileSystem-Überladung verwenden.")]
public LocalMarketDataDrive(string path)
    : this(Paths.FileSystem, path) { }

// Empfohlene Variante
public LocalMarketDataDrive(IFileSystem fileSystem, string path) { }
```

Es wird empfohlen, immer Überladungen mit explizit übergebenem `IFileSystem` zu verwenden, da veraltete Konstruktoren in zukünftigen Versionen entfernt werden.
