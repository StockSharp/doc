# Datenimport

[S#](../api.md) implementiert ein Subsystem zum Import von Marktdaten aus CSV-Dateien. Die wichtigsten Klassen befinden sich im Namespace `StockSharp.Algo.Import`.

## CsvParser - Basisparser

Die Klasse [CsvParser](xref:StockSharp.Algo.Import.CsvParser) parst CSV-Dateien und konvertiert Zeilen in [S#](../api.md)-Nachrichten.

- **Konstruktor**: `(DataType dataType, IEnumerable<FieldMapping> fields)`
- **ColumnSeparator** - Spaltentrennzeichen (Standard `","`).
- **LineSeparator** - Zeilentrennzeichen (Standard CRLF).
- **SkipFromHeader** - Anzahl der Zeilen, die am Dateianfang übersprungen werden (Standard `0`).
- **IgnoreNonIdSecurities** - Zeilen mit nicht erkannten Instrumenten ignorieren (Standard `true`).
- **Parse(Stream)** - Parsingmethode, gibt `IAsyncEnumerable<Message>` zurück.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);
var parser = new CsvParser(DataType.Ticks, fields)
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");

await foreach (var msg in parser.Parse(stream))
{
    // Jede Nachricht verarbeiten
}
```

## CsvImporter - Import mit Speichersicherung

Die Klasse [CsvImporter](xref:StockSharp.Algo.Import.CsvImporter) erweitert [CsvParser](xref:StockSharp.Algo.Import.CsvParser) und fügt die Möglichkeit hinzu, Daten automatisch in einem Marktdatenspeicher abzulegen.

- **Konstruktor**: `(DataType dataType, IEnumerable<FieldMapping> fields, ISecurityStorage securityStorage, IExchangeInfoProvider exchangeInfoProvider, Func<SecurityId, IMarketDataStorage> getStorage)`
- **Import(Stream, Action\<int\> progress, CancellationToken)** - führt den Import aus und gibt `ValueTask<(int count, DateTime? lastTime)>` zurück.
- **UpdateDuplicateSecurities** - ob doppelte Instrumente aktualisiert werden sollen (Standard `false`).
- **SecurityUpdated** - Ereignis, das ausgelöst wird, wenn ein Instrument aktualisiert wird.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);

var importer = new CsvImporter(
    DataType.Ticks,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, DataType.Ticks))
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");
var (count, lastTime) = await importer.Import(
    stream,
    p => Console.WriteLine($"Fortschritt: {p}%"),
    token);

Console.WriteLine($"{count} Datensätze importiert, letzter: {lastTime}");
```

## FieldMapping - Feldbeschreibungen

Die Klasse [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) beschreibt die Zuordnung zwischen CSV-Dateispalten und Nachrichteneigenschaften.

Wichtigste Eigenschaften:

- **Name** - Feldname in der Nachricht.
- **DisplayName** - Anzeigename.
- **Type** - Werttyp.
- **Order** - Spaltenindex in der Datei (beginnend bei 0).
- **IsRequired** - ob das Feld erforderlich ist.
- **Format** - Parsingformat (z. B. Datumsformat).
- **DefaultValue** - Standardwert.
- **ZeroAsNull** - ob Nullwerte als `null` interpretiert werden sollen.

Für benutzerdefinierte Werttransformationen verwenden Sie [FieldMappingValue](xref:StockSharp.Algo.Import.FieldMappingValue). Sie können beispielsweise eine Zuordnung von Textwerten zu Enumerationen definieren:

```cs
var sideField = fields.First(f => f.Name == "Side");
sideField.Order = 3;
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "B",
    ValueTo = Sides.Buy
});
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "S",
    ValueTo = Sides.Sell
});
```

## FieldMappingRegistry - Registry für Standardfelder

Die statische Klasse [FieldMappingRegistry](xref:StockSharp.Algo.Import.FieldMappingRegistry) stellt eine Methode zum Erstellen eines Standardsatzes von Feldern bereit:

- **CreateFields(DataType)** - gibt eine Liste von [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) für den angegebenen Datentyp zurück.

Unterstützte Datentypen: Ticks, Kerzen, Orderbücher, Level1, Orderlog, Transaktionen, Instrumente, Nachrichten und Positionen.

## ImportSettings - Importeinstellungen

Die Klasse [ImportSettings](xref:StockSharp.Algo.Import.ImportSettings) fasst alle Importparameter in einem einzigen Konfigurationsobjekt zusammen:

- **DataType** - Typ der zu importierenden Daten.
- **FileName** - Dateipfad.
- **Directory** - Verzeichnis für die Dateisuche.
- **FileMask** - Suchmaske für Dateien (z. B. `*.csv`).
- **ColumnSeparator** - Spaltentrennzeichen.
- **SkipFromHeader** - Anzahl der zu überspringenden Zeilen.
- **SelectedFields** - für den Import ausgewählte Felder.
- **UpdateDuplicateSecurities** - ob doppelte Instrumente aktualisiert werden sollen.

Hilfsmethoden:

- **GetFiles(IFileSystem)** - Liste der Dateien abrufen, die zur Maske passen.
- **FillParser(CsvParser)** - Parser-Einstellungen ausfüllen.
- **FillImporter(CsvImporter)** - Importer-Einstellungen ausfüllen.

```cs
var settings = new ImportSettings
{
    DataType = DataType.Ticks,
    FileName = "trades.csv",
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

var fields = FieldMappingRegistry.CreateFields(settings.DataType);
// Spaltenreihenfolge konfigurieren
fields[0].Order = 0; // SecurityId
fields[1].Order = 1; // Date
fields[2].Order = 2; // Price
fields[3].Order = 3; // Volume

var importer = new CsvImporter(
    settings.DataType,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, settings.DataType));

settings.FillImporter(importer);

await using var stream = File.OpenRead(settings.FileName);
var (count, lastTime) = await importer.Import(stream, p => { }, token);
```

## Siehe auch

[Datenexport](export.md)

[Datenspeicherung](market_data_storage.md)
