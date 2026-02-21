# Snapshot System

Snapshots in StockSharp represent a mechanism for saving the latest actual state of market data. Instead of scanning the entire history, snapshots allow instantly obtaining the current Level1 value, order book, position, or transaction.

## Purpose of Snapshots

When working with streaming data, it is often necessary to know the latest state of an instrument -- current price, order book, open position. Without snapshots, obtaining this information would require loading and processing the entire history. The snapshot system solves this problem by saving the latest state of each object and providing access to it in minimal time.

## ISnapshotStorage -- Snapshot Storage Interface

The [ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) interface defines the base contract for working with snapshots. The typed version `ISnapshotStorage<TKey, TMessage>` provides the following methods:

- **Update(message)** -- save or update a snapshot. If a snapshot for the given key already exists, it will be updated.
- **Get(key)** -- retrieve a snapshot by key (e.g., by instrument identifier).
- **GetAll(from, to)** -- retrieve all snapshots for the specified date range.
- **Clear(key)** -- delete the snapshot for a specific key.
- **ClearAll()** -- delete all snapshots.

## ISnapshotSerializer -- Snapshot Serialization

The [ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) interface is responsible for converting snapshots to binary representation and back:

- **DataType** -- snapshot data type information.
- **Version** -- serialization format version.
- **Serialize(version, message)** -- serialize a message to a byte array.
- **Deserialize(version, buffer)** -- deserialize a byte array back to a message.
- **GetKey(message)** -- extract the key from a message.
- **Update(message, changes)** -- apply incremental changes to an existing snapshot.

## SnapshotRegistry -- Snapshot Registry

The [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) class is the central component for snapshot management. It implements the `ISnapshotRegistry` interface and coordinates the operation of all snapshot storages.

### Key Features

- **Storage management** -- provides access to snapshot storages for various data types.
- **Periodic writing** -- changes are flushed to disk every 10 seconds, ensuring a balance between performance and reliability.
- **Thread safety** -- all operations are safe for use from multiple threads.

### File Organization

Snapshot files are stored at the following path:

```
{path}/{yyyy_MM_dd}/{serializer_name}.bin
```

## Built-in Serializers

StockSharp includes four serializers for the main market data types:

| Serializer | Message Type | Purpose |
|------------|-------------|---------|
| [Level1BinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.Level1BinarySnapshotSerializer) | `Level1ChangeMessage` | Level1 data (prices, volumes, spreads) |
| [QuotesBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.QuotesBinarySnapshotSerializer) | `QuoteChangeMessage` | Order book |
| [PositionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.PositionBinarySnapshotSerializer) | `PositionChangeMessage` | Positions |
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | Transactions (orders and trades) |

Each serializer supports format versioning, ensuring backward compatibility when updating StockSharp.

## Code Example

### Creating a Snapshot Registry

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### Working with Level1 Snapshots

```cs
// Get the Level1 snapshot storage
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// Save a snapshot
var level1Msg = new Level1ChangeMessage
{
    SecurityId = "SBER@TQBR".ToSecurityId(),
    ServerTime = DateTimeOffset.Now,
};
level1Msg.TryAdd(Level1Fields.LastTradePrice, 260.5m);
level1Msg.TryAdd(Level1Fields.BestBidPrice, 260.4m);
level1Msg.TryAdd(Level1Fields.BestAskPrice, 260.6m);

level1Snapshots.Update(level1Msg);
```

### Retrieving a Snapshot

```cs
// Get the latest snapshot for an instrument
var secId = "SBER@TQBR".ToSecurityId();
var snapshot = level1Snapshots.Get(secId);

if (snapshot != null)
{
    Console.WriteLine($"Last price: {snapshot.Changes[Level1Fields.LastTradePrice]}");
}
```

## See Also

- [Working with the API](api.md)
- [Storage Formats](formats.md)
- [Storage Drives](drives.md)
