# IFileSystem and Paths.FileSystem

## Overview

`IFileSystem` is a file system abstraction used throughout StockSharp. Instead of directly accessing `System.IO.File` and `System.IO.Directory`, platform components work through this interface. This provides:

- **Testability** -- the ability to substitute the file system in unit tests
- **Portability** -- a unified interface for different environments (local disk, cloud storage, memory)
- **Consistency** -- all components use the same approach for working with files

The `IFileSystem` interface is defined in the `Ecng.Common` package and provides methods for working with files and directories: creation, reading, writing, deletion, existence checking, and file enumeration.

## Paths.FileSystem

The `Paths` class (namespace `StockSharp.Configuration`) provides the static `FileSystem` property -- the default `IFileSystem` implementation:

```csharp
public static class Paths
{
    // Standard file system (LocalFileSystem)
    public static readonly IFileSystem FileSystem = Messages.Extensions.DefaultFileSystem;
}
```

`Paths.FileSystem` references `LocalFileSystem.Instance` -- a singleton that works with the local disk via standard `System.IO` facilities.

## Main IFileSystem Methods

The `IFileSystem` interface includes methods for typical operations:

| Method | Description |
|--------|-------------|
| `FileExists(path)` | Check if a file exists |
| `DirectoryExists(path)` | Check if a directory exists |
| `CreateDirectory(path)` | Create a directory |
| `OpenRead(path)` | Open a file for reading (`Stream`) |
| `OpenWrite(path)` | Open a file for writing (`Stream`) |
| `MoveFile(src, dst)` | Move a file |
| `DeleteFile(path)` | Delete a file |
| `EnumerateFiles(path, mask)` | Enumerate files in a directory |
| `WriteAllTextAsync(path, text)` | Write text to a file asynchronously |

## Where It Is Used

Virtually all StockSharp components that work with the file system accept `IFileSystem` in their constructor. Below are the most common cases.

### LocalMarketDataDrive

Local disk market data storage:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Recommended way -- explicitly passing IFileSystem
var drive = new LocalMarketDataDrive(Paths.FileSystem, @"C:\MarketData");

// Deprecated way (uses Paths.FileSystem internally)
// var drive = new LocalMarketDataDrive(@"C:\MarketData"); // [Obsolete]
```

### CsvEntityRegistry

Entity registry (exchanges, securities, portfolios) in CSV format:

```csharp
using StockSharp.Algo.Storages.Csv;
using StockSharp.Configuration;

var executor = new ChannelExecutor();
var registry = new CsvEntityRegistry(Paths.FileSystem, @"C:\Data", executor);
```

### SnapshotRegistry

Market data snapshot registry:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

var snapshots = new SnapshotRegistry(Paths.FileSystem, Paths.SnapshotsDir);
```

### Serialization and Deserialization

Extension methods in `Paths` for working with JSON configurations:

```csharp
using StockSharp.Configuration;

var fs = Paths.FileSystem;

// Serialize an object to a file
settings.Serialize(fs, @"C:\config.json");

// Deserialize an object from a file
var loaded = @"C:\config.json".Deserialize<SettingsStorage>(fs);

// Async deserialization
var data = await @"C:\data.json".DeserializeAsync<MyData>(fs, cancellationToken);

// Check if a configuration file exists
if (@"C:\config.json".IsConfigExists(fs))
{
    // ...
}
```

### CandlePatternFileStorage

Candle pattern storage:

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

var patternStorage = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);
```

### NativeIdStorage, SecurityMappingStorage, and Others

Most mapping and identifier storages also accept `IFileSystem`:

```csharp
// NativeIdStorage
var nativeIdStorage = new NativeIdStorage(Paths.FileSystem, Paths.SecurityNativeIdDir, executor);

// SecurityMappingStorage
var mappingStorage = new SecurityMappingStorage(Paths.FileSystem, Paths.SecurityMappingDir, executor);

// ExtendedInfoStorage
var extInfoStorage = new ExtendedInfoStorage(Paths.FileSystem, Paths.SecurityExtendedInfo, executor);
```

## Typical Usage Pattern

In StockSharp applications, it is recommended to keep a reference to `IFileSystem` and pass it to all components:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Get the file system
var fs = Paths.FileSystem;

// Create data storage
var drive = new LocalMarketDataDrive(fs, Paths.StorageDir);

// Create a connector and configure storage
var connector = new Connector();
connector.Adapter.StorageSettings.Drive = drive;

// Load settings from a file
var configFile = Path.Combine(Paths.AppDataPath, "connector_config.json");

if (configFile.IsConfigExists(fs))
{
    var config = configFile.Deserialize<SettingsStorage>(fs);
    connector.Load(config);
}
```

## Deprecated Overloads

Many classes retain constructors without `IFileSystem` for backward compatibility, but they are marked with the `[Obsolete]` attribute. These constructors use `Paths.FileSystem` internally:

```csharp
// Deprecated way
[Obsolete("Use IFileSystem overload.")]
public LocalMarketDataDrive(string path)
    : this(Paths.FileSystem, path) { }

// Recommended way
public LocalMarketDataDrive(IFileSystem fileSystem, string path) { }
```

It is recommended to always use overloads with explicit `IFileSystem` passing, as deprecated constructors will be removed in future versions.
