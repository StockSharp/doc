# Speicherlaufwerke

Speicherlaufwerke in StockSharp sind für die physische Ablage von Marktdaten verantwortlich - auf einer lokalen Festplatte oder auf einem entfernten Server. Das Basisinterface [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) definiert den gemeinsamen Vertrag für alle Implementierungen.

## IMarketDataDrive - Basisinterface

Das Interface [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) stellt folgende Kernfunktionen bereit:

- **Path** - Pfad zum Datenspeicher.
- **GetAvailableSecuritiesAsync()** - Liste aller verfügbaren Instrumente im Speicher abrufen.
- **GetAvailableDataTypesAsync()** - Liste der für ein bestimmtes Instrument verfügbaren Datentypen abrufen.
- **GetStorageDrive()** - Speicherlaufwerk für ein bestimmtes Instrument und einen bestimmten Datentyp erhalten.
- **VerifyAsync()** - Integrität des Speichers prüfen.
- **LookupSecuritiesAsync()** - Instrumente nach angegebenen Kriterien suchen.

## LocalMarketDataDrive - Lokaler Dateispeicher

Die Klasse [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) ist die primäre Laufwerksimplementierung und speichert Daten auf der lokalen Festplatte im Dateisystem.

### Wichtige Eigenschaften

- **Dateisystem** - Daten werden in einer hierarchischen Verzeichnisstruktur nach Instrumenten und Daten organisiert.
- **Indexsystem** - die interne Klasse `Index` ermöglicht schnellen Datenzugriff ohne Scannen des Dateisystems. Indexdateien werden im Format `{instrument_path}{file_name}Dates2.bin` gespeichert.
- **Threadsicherheit** - der Datenzugriff wird durch Sperrmechanismen geschützt, damit mehrthreadige Anwendungen korrekt arbeiten.
- **Indexerstellung** - die Methode `BuildIndexAsync()` ermöglicht den Neuaufbau von Indizes zur Verbesserung der Performance nach Massenoperationen mit Daten.

### Verwendungsbeispiel

```cs
// Lokales Laufwerk mit angegebenem Pfad erstellen
var localDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));

// Mit der Storage Registry verwenden
var storageRegistry = new StorageRegistry
{
    DefaultDrive = localDrive,
};

// Liste verfügbarer Instrumente abrufen
await foreach (var secId in localDrive.GetAvailableSecuritiesAsync())
{
    Console.WriteLine(secId);
}
```

## RemoteMarketDataDrive - Entfernter Speicher

Die Klasse [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) ermöglicht die Verbindung zu einem entfernten Hydra-Server, um über das Netzwerk auf Marktdaten zuzugreifen.

### Verbindungseinstellungen

- **Adresse** - Adresse des entfernten Servers. Standard ist `127.0.0.1:5002`.
- **Anmeldedaten** - Anmeldedaten (E-Mail und Passwort).
- **TargetCompId** - Ziel-Komponentenbezeichner, standardmäßig `"StockSharpHydraMD"`.
- **SecurityBatchSize** - Batchgröße beim Laden von Instrumenten, standardmäßig 1000.
- **Timeout** - Verbindungs-Timeout, standardmäßig 2 Minuten.

### Verwendungsbeispiel

```cs
// Entferntes Laufwerk erstellen
var remoteDrive = new RemoteMarketDataDrive
{
    Address = "192.168.1.100:5002".To<EndPoint>(),
    Credentials = { Email = "user", Password = "pass".Secure() }
};

// Verfügbare Datentypen für ein Instrument abrufen
var secId = "AAPL@NASDAQ".ToSecurityId();
await foreach (var dataType in remoteDrive.GetAvailableDataTypesAsync(secId, StorageFormats.Binary))
{
    Console.WriteLine(dataType);
}
```

Weitere Details zur Arbeit mit entferntem Speicher finden Sie im Abschnitt [Arbeiten mit Remote Storage](remote.md).

## DriveCache - Laufwerksverwaltung

Die Klasse [DriveCache](xref:StockSharp.Algo.Storages.DriveCache) verwaltet eine Sammlung von Speicherlaufwerken und stellt Caching zur Wiederverwendung bereit.

### Wichtige Methoden und Eigenschaften

- **GetDrive(path)** - vorhandenes Laufwerk nach Pfad abrufen oder ein neues erstellen.
- **DeleteDrive(drive)** - Laufwerk aus dem Cache entfernen.
- **TryDefaultDrive** - das erste verfügbare [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive).
- **NewDriveCreated** - Ereignis beim Erstellen eines neuen Laufwerks.
- **DriveDeleted** - Ereignis beim Löschen eines Laufwerks.
- **Changed** - Ereignis bei Änderungen an der Laufwerkssammlung.

Die Klasse implementiert das Interface `IPersistable`, wodurch Laufwerkskonfigurationen gespeichert und geladen werden können.

### Verwendungsbeispiel

```cs
// Cache mit einem lokalen Standardlaufwerk erstellen
var defaultDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));
var driveCache = new DriveCache(defaultDrive);

// Laufwerk nach Pfad abrufen oder erstellen
var anotherDrive = driveCache.GetDrive(@"D:\MarketData");

// Ereignisse abonnieren
driveCache.NewDriveCreated += drive =>
    Console.WriteLine($"Speicher erstellt: {drive.Path}");
```

## Siehe auch

- [Arbeiten mit der API](api.md)
- [Arbeiten mit Remote Storage](remote.md)
- [Speicherformate](formats.md)

