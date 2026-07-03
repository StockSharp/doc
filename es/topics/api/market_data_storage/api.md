# Trabajo con la API

## Preparación

Para trabajar con datos históricos en los ejemplos, se usa un paquete NuGet con muestras de datos históricos. Se puede instalar desde [NuGet Gallery](https://www.nuget.org/packages/StockSharp.Samples.HistoryData). Este paquete proporciona un conjunto de datos que se puede usar para demostrar el trabajo con el almacenamiento.

Todos los códigos están disponibles en el [repositorio StockSharp](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage).

## Creación de un registro de almacenamiento

Para trabajar con almacenamiento de datos de mercado en StockSharp, se usa la clase [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry). Al crear un objeto de esta clase, puede establecer la ruta al almacenamiento predeterminado mediante la propiedad [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) o especificar una carpeta concreta para trabajar con datos históricos usando [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive).

```cs
// Creación de StorageRegistry con la ruta predeterminada
var storageRegistry = new StorageRegistry();
```

```cs
// Creación de StorageRegistry con la ruta a datos del paquete NuGet
var pathHistory = Paths.HistoryDataPath; // ruta a datos del paquete NuGet
var localDrive = new LocalMarketDataDrive(pathHistory);
var storageRegistry = new StorageRegistry()
{
	DefaultDrive = localDrive,
};
```

## Recuperación de datos

Mediante [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), puede acceder a varios tipos de datos de mercado para el rango de tiempo deseado. Los métodos usados para esto son:

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageHelper.GetTimeFrameCandleMessageStorage(StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) para velas
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) para ticks
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) para libros de órdenes

Cada uno de estos métodos devuelve el almacenamiento correspondiente, desde el que se pueden cargar datos mediante el método `LoadAsync`, especificando las fechas de inicio y fin.

```cs
// Recuperación de velas
var securityId = "AAPL@NASDAQ".ToSecurityId();
var candleStorage = storageRegistry.GetTimeFrameCandleMessageStorage(securityId, TimeSpan.FromMinutes(1), StorageFormats.Binary);
var candles = candleStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var candle in candles)
{
	Console.WriteLine(candle);
}
```

```cs
// Recuperación de ticks
var tradeStorage = storageRegistry.GetTickMessageStorage(securityId, StorageFormats.Binary);
var trades = tradeStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var trade in trades)
{
	Console.WriteLine(trade);
}
```

```cs
// Recuperación de libros de órdenes
var marketDepthStorage = storageRegistry.GetQuoteMessageStorage(securityId, StorageFormats.Binary);
var marketDepths = marketDepthStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var marketDepth in marketDepths)
{
	Console.WriteLine(marketDepth);
}
```

## Guardado de datos

Para guardar datos nuevos en el almacenamiento existente, use el método `SaveAsync` del almacenamiento correspondiente. Esto permite complementar datos históricos con nuevos valores.

```cs
// Guardado de nuevas velas
var newCandles = new List<CandleMessage>
{
	// Aquí se crean nuevos objetos CandleMessage
};
await candleStorage.SaveAsync(newCandles);
```

```cs
// Guardado de nuevos ticks
var newTrades = new List<ExecutionMessage>
{
	// Aquí se crean nuevos objetos ExecutionMessage para ticks
};
await tradeStorage.SaveAsync(newTrades);
```

```cs
// Guardado de nuevos libros de órdenes
var newMarketDepths = new List<QuoteChangeMessage>
{
	// Aquí se crean nuevos objetos QuoteChangeMessage para libros de órdenes
};
await marketDepthStorage.SaveAsync(newMarketDepths);
```

## Eliminación de datos

Para eliminar datos de un período específico, use el método `DeleteAsync` del almacenamiento correspondiente. Tenga cuidado al eliminar datos del paquete de muestras.

```cs
// Eliminación de velas del período especificado
await candleStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Eliminación de ticks del período especificado
await tradeStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Eliminación de libros de órdenes del período especificado
await marketDepthStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

Estas operaciones le permiten gestionar de forma eficaz datos históricos, tanto si se cargaron mediante [Hydra](../../hydra.md), se proporcionaron en el paquete NuGet o se crearon durante el funcionamiento de su aplicación.
