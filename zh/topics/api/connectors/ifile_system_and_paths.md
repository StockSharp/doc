# IFileSystem 和 Paths.FileSystem

## 概览

`IFileSystem` 是在整个 StockSharp 中使用的文件系统抽象。平台组件不是直接访问 `System.IO.File` 和 `System.IO.Directory`，而是通过这个接口工作。这提供了：

- **可测试性**——在单元测试中替换文件系统的能力
- **可移植性** —— 不同环境（本地磁盘、云存储、内存）的统一接口
- **一致性** -- 所有组件在处理文件时使用相同的方法

`IFileSystem` 接口在 `Ecng.Common` 包中定义，并提供用于处理文件和目录的方法：创建、读取、写入、删除、存在性检查和文件枚举。

## Paths.FileSystem

`Paths` 类（命名空间 `StockSharp.Configuration`）提供静态 `FileSystem` 属性 —— 默认的 `IFileSystem` 实现：

```csharp
public static class Paths
{
    // Standard file system (LocalFileSystem)
    public static readonly IFileSystem FileSystem = Messages.Extensions.DefaultFileSystem;
}
```

`Paths.FileSystem` 引用 `LocalFileSystem.Instance` —— 一个通过标准 `System.IO` 功能与本地磁盘协作的单例。

## 主要 IFileSystem 方法

`IFileSystem` 接口包含用于典型操作的方法：

| 方法 | 描述 |
|--------|-------------|
| `FileExists(path)` | 检查文件是否存在 |
| `DirectoryExists(path)` | 检查目录是否存在 |
| `CreateDirectory(path)` | 创建一个目录 |
| `OpenRead(path)` | 打开文件以进行读取 (`Stream`) |
| `OpenWrite(path)` | 打开文件以进行写入 (`Stream`) |
| `MoveFile(src, dst)` | 移动一个文件 |
| `DeleteFile(path)` | 删除文件 |
| `EnumerateFiles(path, mask)` | 枚举目录中的文件 |
| `WriteAllTextAsync(path, text)` | 异步写入文本到文件 |

## 它的使用地点

几乎所有与文件系统一起工作的 StockSharp 组件在其构造函数中都接受 `IFileSystem`。以下是最常见的情况。

### 本地市场数据驱动

本地磁盘市场数据存储：

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Recommended way -- explicitly passing IFileSystem
var drive = new LocalMarketDataDrive(Paths.FileSystem, @"C:\MarketData");

// Deprecated way (uses Paths.FileSystem internally)
// var drive = new LocalMarketDataDrive(@"C:\MarketData"); // [Obsolete]
```

### Csv实体注册表

实体注册表（交易所、证券、投资组合）CSV 格式：

```csharp
using StockSharp.Algo.Storages.Csv;
using StockSharp.Configuration;

var executor = new ChannelExecutor();
var registry = new CsvEntityRegistry(Paths.FileSystem, @"C:\Data", executor);
```

### 快照注册表

市场数据快照注册表：

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

var snapshots = new SnapshotRegistry(Paths.FileSystem, Paths.SnapshotsDir);
```

### 序列化和反序列化

`Paths` 中用于处理 JSON 配置的扩展方法：

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

### 蜡烛图模式文件存储

蜡烛图形存储：

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

var patternStorage = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);
```

### NativeIdStorage、安全映射存储等

大多数映射和标识符存储也接受 `IFileSystem`:

```csharp
// NativeIdStorage
var nativeIdStorage = new NativeIdStorage(Paths.FileSystem, Paths.SecurityNativeIdDir, executor);

// SecurityMappingStorage
var mappingStorage = new SecurityMappingStorage(Paths.FileSystem, Paths.SecurityMappingDir, executor);

// ExtendedInfoStorage
var extInfoStorage = new ExtendedInfoStorage(Paths.FileSystem, Paths.SecurityExtendedInfo, executor);
```

## 典型使用模式

在 StockSharp 应用中，建议保留对 `IFileSystem` 的引用，并将其传递给所有组件：

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

## 已弃用的重载

许多类保留了没有 `IFileSystem` 的构造函数以保持向后兼容性，但它们被标记了 `[Obsolete]` 属性。这些构造函数在内部使用 `Paths.FileSystem`：

```csharp
// Deprecated way
[Obsolete("Use IFileSystem overload.")]
public LocalMarketDataDrive(string path)
    : this(Paths.FileSystem, path) { }

// Recommended way
public LocalMarketDataDrive(IFileSystem fileSystem, string path) { }
```

建议始终使用带有显式 `IFileSystem` 传递的重载方法，因为已弃用的构造函数将在未来版本中被移除。
