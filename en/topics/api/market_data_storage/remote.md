# Working with Remote Storage

## Introduction

In addition to local storage, the API provides the capability to work with remote market data storage. This is especially useful when using Hydra in [server mode](../../hydra/server_mode/settings.md) or when connecting to a [Hydra server](../../hydra_server.md).

## Connecting to Remote Storage

To work with remote storage, use the [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) class.

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

## Loading Instrument Information

Before loading market data, you need to get information about available instruments.

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

## Loading Market Data

After obtaining instrument information, you can proceed to load market data.

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

## Saving Data Locally

Loaded data can be saved locally for further use.

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

## Using Data for Testing

Loaded and locally saved data can be used for testing trading strategies with the [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector).

```cs
// Using HistoryEmulationConnector
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## Obtaining Available Date Ranges

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

## Conclusion

The API's remote market data storage functionality provides flexible capabilities for obtaining and using historical data. This allows for efficient testing of trading strategies and market analysis using extensive datasets available through the [Hydra server](../../hydra_server.md).