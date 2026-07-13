# Trabajo con almacenamiento remoto

## Introducción

Además del almacenamiento local, la API proporciona la capacidad de trabajar con almacenamiento remoto de datos de mercado. Esto es especialmente útil al usar Hydra en [modo servidor](../../hydra/server_mode/settings.md) o al conectarse a un [servidor Hydra](../../hydra_server.md).

## Conexión al almacenamiento remoto

Para trabajar con almacenamiento remoto, use la clase [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive).

```cs
// Creación de RemoteMarketDataDrive
var remoteDrive = new RemoteMarketDataDrive(RemoteMarketDataDrive.DefaultAddress, new FixMessageAdapter(new IncrementalIdGenerator()))
{
	Credentials = { Email = "hydra_user", Password = "hydra_user".To<SecureString>() }
};

// Este código crea una instancia de RemoteMarketDataDrive para conectarse al almacenamiento remoto.
// Usa la dirección predeterminada y FixMessageAdapter para la comunicación.
// Las credenciales se establecen para autenticación.
```

## Carga de información de instrumentos

Antes de cargar datos de mercado, necesita obtener información sobre los instrumentos disponibles.

```cs
// Carga de información de instrumentos
var exchangeInfoProvider = new InMemoryExchangeInfoProvider();
remoteDrive.LookupSecurities(Extensions.LookupAllCriteriaMessage, registry.Securities,
	s => securityStorage.Save(s.ToSecurity(exchangeInfoProvider), false), () => false,
	(c, t) => Console.WriteLine($"Descargado [{c}]/[{t}]"));

var securities = securityStorage.LookupAll();

// Este código carga información sobre todos los instrumentos disponibles desde el almacenamiento remoto.
// Los instrumentos cargados se guardan en el almacenamiento local y se muestran en la consola.
```

## Carga de datos de mercado

Después de obtener información de instrumentos, puede proceder a cargar datos de mercado.

```cs
// Carga de datos de mercado
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	var localStorage = storageRegistry.GetStorage(secId, dataType.MessageType, dataType.Arg, localDrive, format);
	var remoteStorage = remoteDrive.GetStorageDrive(secId, dataType, format);

	// ... (código de carga de datos)
}

// Este bucle recorre todos los tipos de datos disponibles para el instrumento especificado.
// Para cada tipo de datos, se crea un almacenamiento local y se accede al almacenamiento remoto.
```

## Guardado local de datos

Los datos cargados se pueden guardar localmente para su uso posterior.

```cs
// Guardado de datos localmente
foreach (var dateTime in dates)
{
	using (var stream = remoteStorage.LoadStream(dateTime))
	{
		if (stream == Stream.Null)
			continue;

		localStorage.Drive.SaveStream(dateTime, stream);
	}

	// ... (código de salida de datos)
}

// Este código carga datos para cada fecha desde el almacenamiento remoto y los guarda en almacenamiento local.
```

## Uso de datos para pruebas

Los datos cargados y guardados localmente se pueden usar para probar estrategias de negociación con [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector).

```cs
// Uso de HistoryEmulationConnector
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## Obtención de rangos de fechas disponibles

```cs
// Trabajo con varios tipos de datos
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	// ... (código de procesamiento de datos)

	// Manejo de errores y registro
	Console.WriteLine($"Remote {dataType}: {remoteStorage.Dates.FirstOrDefault()}-{remoteStorage.Dates.LastOrDefault()}");
	Console.WriteLine($"{dataType}={dateTime}");
}
```

## Conclusión

La funcionalidad de almacenamiento remoto de datos de mercado de la API proporciona capacidades flexibles para obtener y usar datos históricos. Esto permite realizar pruebas eficientes de estrategias de negociación y análisis de mercado usando amplios conjuntos de datos disponibles mediante el [servidor Hydra](../../hydra_server.md).
