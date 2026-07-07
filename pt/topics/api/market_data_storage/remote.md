# Trabalhar com Armazenamento Remoto

## Introdução

Além do armazenamento local, a API disponibiliza a capacidade de trabalhar com armazenamento remoto de dados de mercado. Isto é especialmente útil ao utilizar o Hydra em [modo servidor](../../hydra/server_mode/settings.md) ou ao ligar a um [servidor Hydra](../../hydra_server.md).

## Ligar ao Armazenamento Remoto

Para trabalhar com armazenamento remoto, use a classe [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive).

```cs
// Creating RemoteMarketDataDrive
var remoteDrive = new RemoteMarketDataDrive(RemoteMarketDataDrive.DefaultAddress, new FixMessageAdapter(new IncrementalIdGenerator()))
{
	Credentials = { Email = "hydra_user", Password = "hydra_user".To<SecureString>() }
};

// This code creates an instance of RemoteMarketDataDrive to connect to remote storage.
// It uses the default address and FixMessageAdapter for communication.
// Credentials are set for authentication.
```

## Carregar Informação de Instrumentos

Antes de carregar dados de mercado, é necessário obter informação sobre os instrumentos disponíveis.

```cs
// Loading instrument information
var exchangeInfoProvider = new InMemoryExchangeInfoProvider();
remoteDrive.LookupSecurities(Extensions.LookupAllCriteriaMessage, registry.Securities,
	s => securityStorage.Save(s.ToSecurity(exchangeInfoProvider), false), () => false,
	(c, t) => Console.WriteLine($"Downloaded [{c}]/[{t}]"));

var securities = securityStorage.LookupAll();

// This code loads information about all available instruments from the remote storage.
// The loaded instruments are saved in local storage and output to the console.
```

## Carregar Dados de Mercado

Depois de obter a informação dos instrumentos, pode avançar para o carregamento dos dados de mercado.

```cs
// Loading market data
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	var localStorage = storageRegistry.GetStorage(secId, dataType.MessageType, dataType.Arg, localDrive, format);
	var remoteStorage = remoteDrive.GetStorageDrive(secId, dataType, format);

	// ... (data loading code)
}

// This loop iterates through all available data types for the specified instrument.
// For each data type, a local storage is created and remote storage is accessed.
```

## Guardar Dados Localmente

Os dados carregados podem ser guardados localmente para utilização posterior.

```cs
// Saving data locally
foreach (var dateTime in dates)
{
	using (var stream = remoteStorage.LoadStream(dateTime))
	{
		if (stream == Stream.Null)
			continue;

		localStorage.Drive.SaveStream(dateTime, stream);
	}

	// ... (data output code)
}

// This code loads data for each date from the remote storage and saves it to local storage.
```

## Usar Dados para Testes

Os dados carregados e guardados localmente podem ser usados para testar estratégias de negociação com o [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector).

```cs
// Using HistoryEmulationConnector
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## Obter Intervalos de Datas Disponíveis

```cs
// Working with various data types
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	// ... (data processing code)

	// Error handling and logging
	Console.WriteLine($"Remote {dataType}: {remoteStorage.Dates.FirstOrDefault()}-{remoteStorage.Dates.LastOrDefault()}");
	Console.WriteLine($"{dataType}={dateTime}");
}
```

## Conclusão

A funcionalidade de armazenamento remoto de dados de mercado da API fornece capacidades flexíveis para obter e usar dados históricos. Isto permite testar estratégias de negociação e analisar o mercado de forma eficiente usando conjuntos de dados extensos disponíveis através do [servidor Hydra](../../hydra_server.md).
