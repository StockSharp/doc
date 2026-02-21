# Storage Drives

Storage drives in StockSharp are responsible for the physical placement of market data -- on a local disk or on a remote server. The base interface [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) defines the common contract for all implementations.

## IMarketDataDrive -- Base Interface

The [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) interface provides the following key capabilities:

- **Path** -- path to the data storage.
- **GetAvailableSecuritiesAsync()** -- retrieve a list of all available instruments in the storage.
- **GetAvailableDataTypesAsync()** -- retrieve a list of data types available for a specific instrument.
- **GetStorageDrive()** -- obtain a storage drive for a specific instrument and data type.
- **VerifyAsync()** -- verify the integrity of the storage.
- **LookupSecuritiesAsync()** -- search for instruments by specified criteria.

## LocalMarketDataDrive -- Local File Storage

The [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) class is the primary drive implementation that stores data on the local disk in the file system.

### Key Features

- **File system** -- data is organized in a hierarchical directory structure by instruments and dates.
- **Index system** -- the internal `Index` class provides fast data access without scanning the file system. Index files are stored in the format `{instrument_path}{file_name}Dates2.bin`.
- **Thread safety** -- data access is protected by locking mechanisms for correct operation in multi-threaded applications.
- **Index building** -- the `BuildIndexAsync()` method allows rebuilding indexes for improved performance after bulk data operations.

### Usage Example

```cs
// Create a local drive with a specified path
var localDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));

// Use with the storage registry
var storageRegistry = new StorageRegistry
{
    DefaultDrive = localDrive,
};

// Retrieve the list of available instruments
await foreach (var secId in localDrive.GetAvailableSecuritiesAsync())
{
    Console.WriteLine(secId);
}
```

## RemoteMarketDataDrive -- Remote Storage

The [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) class allows connecting to a remote Hydra server for accessing market data over the network.

### Connection Settings

- **Address** -- remote server address. Default is `127.0.0.1:5002`.
- **Credentials** -- authentication credentials (Email and Password).
- **TargetCompId** -- target component identifier, default is `"StockSharpHydraMD"`.
- **SecurityBatchSize** -- batch size when loading instruments, default is 1000.
- **Timeout** -- connection timeout, default is 2 minutes.

### Usage Example

```cs
// Create a remote drive
var remoteDrive = new RemoteMarketDataDrive
{
    Address = "192.168.1.100:5002".To<EndPoint>(),
    Credentials = { Email = "user", Password = "pass".Secure() }
};

// Retrieve available data types for an instrument
var secId = "SBER@TQBR".ToSecurityId();
await foreach (var dataType in remoteDrive.GetAvailableDataTypesAsync(secId, StorageFormats.Binary))
{
    Console.WriteLine(dataType);
}
```

For more details on working with remote storage, see the [Working with Remote Storage](remote.md) section.

## DriveCache -- Drive Management

The [DriveCache](xref:StockSharp.Algo.Storages.DriveCache) class manages a collection of storage drives and provides caching for reuse.

### Key Methods and Properties

- **GetDrive(path)** -- get an existing drive by path or create a new one.
- **DeleteDrive(drive)** -- remove a drive from the cache.
- **TryDefaultDrive** -- the first available [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive).
- **NewDriveCreated** -- event for new drive creation.
- **DriveDeleted** -- event for drive deletion.
- **Changed** -- event for drive collection changes.

The class implements the `IPersistable` interface, which allows saving and loading drive configurations.

### Usage Example

```cs
// Create a cache with a default local drive
var defaultDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));
var driveCache = new DriveCache(defaultDrive);

// Get or create a drive by path
var anotherDrive = driveCache.GetDrive(@"D:\MarketData");

// Subscribe to events
driveCache.NewDriveCreated += drive =>
    Console.WriteLine($"Drive created: {drive.Path}");
```

## See Also

- [Working with the API](api.md)
- [Working with Remote Storage](remote.md)
- [Storage Formats](formats.md)
