# スナップショットシステム

StockSharp のスナップショットは、市場データの最新の実際の状態を保存するためのメカニズムです。履歴全体をスキャンする代わりに、スナップショットにより、現在の Level1 値、板情報、ポジション、またはトランザクションを即座に取得できます。

## スナップショットの目的

ストリーミングデータを扱うときは、現在価格、板情報、オープンポジションなど、銘柄の最新状態を知る必要がよくあります。スナップショットがない場合、この情報を取得するには履歴全体を読み込んで処理する必要があります。スナップショットシステムは、各オブジェクトの最新状態を保存し、最小限の時間でアクセスできるようにすることで、この問題を解決します。

## ISnapshotStorage -- スナップショットストレージインターフェイス

[ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) インターフェイスは、スナップショットを扱うための基本契約を定義します。型付きバージョン `ISnapshotStorage<TKey, TMessage>` は、次のメソッドを提供します。

- **Update(message)** -- スナップショットを保存または更新します。指定されたキーのスナップショットが既に存在する場合は更新されます。
- **Get(key)** -- キー（たとえば銘柄識別子）によってスナップショットを取得します。
- **GetAll(from, to)** -- 指定した日付範囲のすべてのスナップショットを取得します。
- **Clear(key)** -- 特定キーのスナップショットを削除します。
- **ClearAll()** -- すべてのスナップショットを削除します。

## ISnapshotSerializer -- スナップショットのシリアライズ

[ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) インターフェイスは、スナップショットをバイナリ表現へ変換し、また元に戻す役割を担います。

- **DataType** -- スナップショットデータ型情報。
- **Version** -- シリアライズ形式のバージョン。
- **Serialize(version, message)** -- メッセージをバイト配列にシリアライズします。
- **Deserialize(version, buffer)** -- バイト配列をメッセージにデシリアライズします。
- **GetKey(message)** -- メッセージからキーを抽出します。
- **Update(message, changes)** -- 既存のスナップショットに差分変更を適用します。

## SnapshotRegistry -- スナップショットレジストリ

[SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) クラスは、スナップショット管理の中心コンポーネントです。`ISnapshotRegistry` インターフェイスを実装し、すべてのスナップショットストレージの動作を調整します。

### 主な機能

- **ストレージ管理** -- さまざまなデータ型のスナップショットストレージへのアクセスを提供します。
- **定期書き込み** -- 変更は 10 秒ごとにディスクへフラッシュされ、パフォーマンスと信頼性のバランスを確保します。
- **スレッドセーフ** -- すべての操作は、複数スレッドから安全に使用できます。

### ファイル構成

スナップショットファイルは、次のパスに保存されます。

```
{path}/{yyyy_MM_dd}/{serializer_name}.bin
```

## 組み込みシリアライザー

StockSharp には、主要な市場データ型用に 4 つのシリアライザーが含まれています。

| シリアライザー | メッセージ型 | 目的 |
|------------|-------------|---------|
| [Level1BinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.Level1BinarySnapshotSerializer) | `Level1ChangeMessage` | Level1 データ（価格、出来高、スプレッド） |
| [QuotesBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.QuotesBinarySnapshotSerializer) | `QuoteChangeMessage` | 板情報 |
| [PositionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.PositionBinarySnapshotSerializer) | `PositionChangeMessage` | ポジション |
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | トランザクション（注文と取引） |

各シリアライザーは形式のバージョン管理をサポートしており、StockSharp の更新時の後方互換性を確保します。

## コード例

### スナップショットレジストリの作成

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### Level1 スナップショットの操作

```cs
// Level1 スナップショットストレージを取得します
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// スナップショットを保存します
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

### スナップショットの取得

```cs
// 銘柄の最新スナップショットを取得します
var secId = "AAPL@NASDAQ".ToSecurityId();
var snapshot = level1Snapshots.Get(secId);

if (snapshot != null)
{
    Console.WriteLine($"Last price: {snapshot.Changes[Level1Fields.LastTradePrice]}");
}
```

## 関連項目

- [API の操作](api.md)
- [ストレージ形式](formats.md)
- [ストレージドライブ](drives.md)

