# 存储驱动器

StockSharp中的存储驱动负责市场数据的物理存放——无论是在本地磁盘还是在远程服务器上。基础接口[IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive)定义了所有实现的通用契约。

## IMarketDataDrive -- 基础接口

[IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) 接口提供以下关键功能：

- **路径** -- 数据存储的路径。
- **GetAvailableSecuritiesAsync()** -- 检索存储中所有可用工具的列表。
- **GetAvailableDataTypesAsync()** -- 检索特定仪器可用的数据类型列表。
- **GetStorageDrive()** -- 获取特定仪器和数据类型的存储驱动器。
- **VerifyAsync()** -- 验证存储的完整性。
- **LookupSecuritiesAsync()** -- 根据指定条件搜索金融工具。

## LocalMarketDataDrive -- 本地文件存储

[LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) 类是主要的驱动实现，用于在文件系统的本地磁盘上存储数据。

### 主要特点

- **文件系统** -- 数据按仪器和日期组织在分层目录结构中。
- **索引系统** -- 内部的 `Index` 类提供无需扫描文件系统的快速数据访问。索引文件以 `{instrument_path}{file_name}Dates2.bin` 格式存储。
- **线程安全** —— 数据访问通过锁机制保护，以确保在多线程应用中的正确操作。
- **索引构建** -- `BuildIndexAsync()` 方法允许在批量数据操作后重建索引以提高性能。

### 使用示例

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

## 远程市场数据驱动器 -- 远程存储

[RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) 类允许连接到远程 Hydra 服务器以通过网络访问市场数据。

### 连接设置

- **地址** -- 远程服务器地址。默认是 `127.0.0.1:5002`。
- **凭证** -- 认证凭证（电子邮件和密码）。
- **TargetCompId** -- 目标组件标识符，默认值为 `"StockSharpHydraMD"`。
- **SecurityBatchSize** -- 加载工具时的批量大小，默认值为 1000。
- **超时** -- 连接超时，默认是2分钟。

### 使用示例

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

有关使用远程存储的更多详细信息，请参阅 [使用远程存储](remote.md) 部分。

## DriveCache -- 磁盘管理

[DriveCache](xref:StockSharp.Algo.Storages.DriveCache) 类管理存储驱动器的集合并提供缓存以便重复使用。

### 关键方法和属性

- **GetDrive(path)** -- 通过路径获取现有驱动器或创建一个新的驱动器。
- **DeleteDrive(drive)** -- 从缓存中移除一个驱动器。
- **TryDefaultDrive** -- 第一个可用的 [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive)。
- **NewDriveCreated** -- 新驱动器创建事件。
- **DriveDeleted** -- 驱动器删除的事件。
- **已更改**——驱动器收集更改的事件。

该类实现了 `IPersistable` 接口，该接口允许保存和加载驱动器配置。

### 使用示例

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

## 另请参阅

- [使用 API](api.md)
- [使用远程存储](remote.md)
- [存储格式](formats.md)
