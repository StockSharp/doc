# Arbeiten mit Remote Storage

## Einführung

Zusätzlich zum lokalen Speicher bietet die API die Möglichkeit, mit entferntem Marktdatenspeicher zu arbeiten. Dies ist besonders nützlich, wenn Hydra im [Servermodus](../../hydra/server_mode/settings.md) verwendet wird oder wenn eine Verbindung zu einem [Hydra-Server](../../hydra_server.md) hergestellt wird.

## Verbindung zu Remote Storage herstellen

Für die Arbeit mit entferntem Speicher verwenden Sie die Klasse [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive).

```cs
// RemoteMarketDataDrive erstellen
var remoteDrive = new RemoteMarketDataDrive(RemoteMarketDataDrive.DefaultAddress, new FixMessageAdapter(new IncrementalIdGenerator()))
{
	Credentials = { Email = "hydra_user", Password = "hydra_user".To<SecureString>() }
};

// Dieser Code erstellt eine Instanz von RemoteMarketDataDrive für die Verbindung mit entferntem Speicher.
// Er verwendet die Standardadresse und FixMessageAdapter für die Kommunikation.
// Anmeldedaten werden für die Authentifizierung gesetzt.
```

## Instrumenteninformationen laden

Vor dem Laden von Marktdaten müssen Sie Informationen über verfügbare Instrumente abrufen.

```cs
// Instrumenteninformationen laden
var exchangeInfoProvider = new InMemoryExchangeInfoProvider();
remoteDrive.LookupSecurities(Extensions.LookupAllCriteriaMessage, registry.Securities,
	s => securityStorage.Save(s.ToSecurity(exchangeInfoProvider), false), () => false,
	(c, t) => Console.WriteLine($"Downloaded [{c}]/[{t}]"));

var securities = securityStorage.LookupAll();

// Dieser Code lädt Informationen über alle verfügbaren Instrumente aus dem entfernten Speicher.
// Die geladenen Instrumente werden im lokalen Speicher gespeichert und in der Konsole ausgegeben.
```

## Marktdaten laden

Nach dem Abrufen der Instrumenteninformationen können Sie mit dem Laden von Marktdaten fortfahren.

```cs
// Marktdaten laden
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	var localStorage = storageRegistry.GetStorage(secId, dataType.MessageType, dataType.Arg, localDrive, format);
	var remoteStorage = remoteDrive.GetStorageDrive(secId, dataType, format);

	// ... (Code zum Laden der Daten)
}

// Diese Schleife durchläuft alle verfügbaren Datentypen für das angegebene Instrument.
// Für jeden Datentyp wird ein lokaler Speicher erstellt und auf den entfernten Speicher zugegriffen.
```

## Daten lokal speichern

Geladene Daten können zur späteren Verwendung lokal gespeichert werden.

```cs
// Daten lokal speichern
foreach (var dateTime in dates)
{
	using (var stream = remoteStorage.LoadStream(dateTime))
	{
		if (stream == Stream.Null)
			continue;

		localStorage.Drive.SaveStream(dateTime, stream);
	}

	// ... (Code zur Datenausgabe)
}

// Dieser Code lädt Daten für jedes Datum aus dem entfernten Speicher und speichert sie lokal.
```

## Daten für Tests verwenden

Geladene und lokal gespeicherte Daten können zum Testen von Handelsstrategien mit [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) verwendet werden.

```cs
// HistoryEmulationConnector verwenden
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## Verfügbare Datumsbereiche abrufen

```cs
// Arbeit mit verschiedenen Datentypen
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	// ... (Code zur Datenverarbeitung)

	// Fehlerbehandlung und Logging
	Console.WriteLine($"Remote {dataType}: {remoteStorage.Dates.FirstOrDefault()}-{remoteStorage.Dates.LastOrDefault()}");
	Console.WriteLine($"{dataType}={dateTime}");
}
```

## Fazit

Die Funktionalität der API für entfernten Marktdatenspeicher bietet flexible Möglichkeiten zum Abrufen und Verwenden historischer Daten. Dadurch können Handelsstrategien effizient getestet und Marktanalysen mit umfangreichen Datensätzen durchgeführt werden, die über den [Hydra-Server](../../hydra_server.md) verfügbar sind.

