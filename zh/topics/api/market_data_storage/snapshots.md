# 快照系统

StockSharp 中的快照是一种保存市场数据最新实际状态的机制。快照可以立即获取当前的 Level1 值、订单簿、持仓或交易，而无需扫描整个历史数据。

## 快照的用途

在处理流数据时，通常需要知道一个工具的最新状态——当前价格、订单簿、未平仓持仓。如果没有快照，要获取这些信息就需要加载和处理整个历史记录。快照系统通过保存每个对象的最新状态并在最短时间内提供访问来解决这个问题。

## ISnapshotStorage -- 快照存储接口

[ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) 接口定义了用于处理快照的基本契约。类型化版本 `ISnapshotStorage<TKey, TMessage>` 提供以下方法：

- **Update(message)** -- 保存或更新快照。如果给定键的快照已存在，将会更新该快照。
- **Get(key)** -- 通过键获取快照（e.g., 按工具标识符）。
- **GetAll(from, to)** -- 获取指定日期范围内的所有快照。
- **Clear(key)** -- 删除特定键的快照。
- **ClearAll()** -- 删除所有快照。

## ISnapshotSerializer -- 快照序列化

[ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) 接口负责将快照转换为二进制表示并进行还原：

- **数据类型** -- 快照数据类型信息。
- **版本** -- 序列化格式版本。
- **Serialize(version, message)** -- 将消息序列化为字节数组。
- **Deserialize(version, buffer)** -- 将字节数组反序列化回消息。
- **GetKey(message)** -- 从消息中提取密钥。
- **Update(message, changes)** -- 对现有快照应用增量更改。

## SnapshotRegistry -- 快照注册表

[SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) 类是快照管理的核心组件。它实现了 `ISnapshotRegistry` 接口，并协调所有快照存储的操作。

### 主要特点

- **存储管理** -- 提供对各种数据类型快照存储的访问。
- **周期性写入**——更改每10秒刷新一次到磁盘，确保性能与可靠性之间的平衡。
- **线程安全** -- 所有操作都可以安全地被多个线程使用。

### 文件组织

快照文件存储在以下路径：

```
{path}/{yyyy_MM_dd}/{serializer_name}.bin
```

## 内置序列化器

StockSharp 包含四个用于主要市场数据类型的序列化器：

| 序列化器 | 消息类型 | 目的 |
|------------|-------------|---------|
| [Level1BinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.Level1BinarySnapshotSerializer) | `Level1ChangeMessage` | 一级数据（价格、成交量、价差） |
| [QuotesBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.QuotesBinarySnapshotSerializer) | `QuoteChangeMessage` | 订单簿 |
| [PositionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.PositionBinarySnapshotSerializer) | `PositionChangeMessage` | 持仓 |
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | 交易（订单和交易） |

每个序列化器都支持格式版本控制，在更新 StockSharp 时确保向后兼容性。

## 代码示例

### 创建快照注册表

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### 使用 Level1 快照

```cs
// Get the Level1 snapshot storage
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// Save a snapshot
var level1Msg = new Level1ChangeMessage
{
    SecurityId = "AAPL@NASDAQ".ToSecurityId(),
    ServerTime = DateTimeOffset.Now,
};
level1Msg.TryAdd(Level1Fields.LastTradePrice, 260.5m);
level1Msg.TryAdd(Level1Fields.BestBidPrice, 260.4m);
level1Msg.TryAdd(Level1Fields.BestAskPrice, 260.6m);

level1Snapshots.Update(level1Msg);
```

### 正在检索快照

```cs
// Get the latest snapshot for an instrument
var secId = "AAPL@NASDAQ".ToSecurityId();
var snapshot = level1Snapshots.Get(secId);

if (snapshot != null)
{
    Console.WriteLine($"Last price: {snapshot.Changes[Level1Fields.LastTradePrice]}");
}
```

## 另请参阅

- [使用 API](api.md)
- [存储格式](formats.md)
- [存储驱动器](drives.md)
