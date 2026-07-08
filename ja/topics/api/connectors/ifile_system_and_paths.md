# IFileSystem と Paths.FileSystem

## 概要

`IFileSystem` は、StockSharp 全体で使用されるファイルシステム抽象化です。`System.IO.File` や `System.IO.Directory` に直接アクセスする代わりに、プラットフォームのコンポーネントはこのインターフェイスを介して動作します。これにより、次の利点があります。

- **テスト容易性** -- 単体テストでファイルシステムを差し替えられること
- **移植性** -- さまざまな環境（ローカルディスク、クラウドストレージ、メモリ）に対する統一インターフェイス
- **一貫性** -- すべてのコンポーネントがファイル操作に同じアプローチを使用すること

`IFileSystem` インターフェイスは `Ecng.Common` パッケージで定義されており、ファイルとディレクトリを扱うためのメソッドを提供します。作成、読み取り、書き込み、削除、存在確認、ファイル列挙などです。

## Paths.FileSystem

`Paths` クラス（名前空間 `StockSharp.Configuration`）は、既定の `IFileSystem` 実装である静的な `FileSystem` プロパティを提供します。

```csharp
public static class Paths
{
    // 標準ファイルシステム (LocalFileSystem)
    public static readonly IFileSystem FileSystem = Messages.Extensions.DefaultFileSystem;
}
```

`Paths.FileSystem` は `LocalFileSystem.Instance` を参照します。これは、標準の `System.IO` 機能を介してローカルディスクを操作するシングルトンです。

## 主な IFileSystem メソッド

`IFileSystem` インターフェイスには、一般的な操作のためのメソッドが含まれています。

| メソッド | 説明 |
|--------|-------------|
| `FileExists(path)` | ファイルが存在するか確認します |
| `DirectoryExists(path)` | ディレクトリが存在するか確認します |
| `CreateDirectory(path)` | ディレクトリを作成します |
| `OpenRead(path)` | 読み取り用にファイルを開きます（`Stream`） |
| `OpenWrite(path)` | 書き込み用にファイルを開きます（`Stream`） |
| `MoveFile(src, dst)` | ファイルを移動します |
| `DeleteFile(path)` | ファイルを削除します |
| `EnumerateFiles(path, mask)` | ディレクトリ内のファイルを列挙します |
| `WriteAllTextAsync(path, text)` | テキストを非同期でファイルに書き込みます |

## 使用される場所

ファイルシステムを扱う StockSharp コンポーネントのほぼすべては、コンストラクターで `IFileSystem` を受け取ります。以下は最も一般的なケースです。

### LocalMarketDataDrive

ローカルディスク上のマーケットデータストレージ:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// 推奨される方法 -- IFileSystem を明示的に渡す
var drive = new LocalMarketDataDrive(Paths.FileSystem, @"C:\MarketData");

// 非推奨の方法 (内部で Paths.FileSystem を使用)
// var drive = new LocalMarketDataDrive(@"C:\MarketData"); // [Obsolete]
```

### CsvEntityRegistry

CSV 形式のエンティティレジストリ（取引所、銘柄、ポートフォリオ）:

```csharp
using StockSharp.Algo.Storages.Csv;
using StockSharp.Configuration;

var executor = new ChannelExecutor();
var registry = new CsvEntityRegistry(Paths.FileSystem, @"C:\Data", executor);
```

### SnapshotRegistry

マーケットデータのスナップショットレジストリ:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

var snapshots = new SnapshotRegistry(Paths.FileSystem, Paths.SnapshotsDir);
```

### シリアル化と逆シリアル化

JSON 設定を扱うための `Paths` の拡張メソッド:

```csharp
using StockSharp.Configuration;

var fs = Paths.FileSystem;

// オブジェクトをファイルへシリアル化する
settings.Serialize(fs, @"C:\config.json");

// ファイルからオブジェクトを逆シリアル化する
var loaded = @"C:\config.json".Deserialize<SettingsStorage>(fs);

// 非同期の逆シリアル化
var data = await @"C:\data.json".DeserializeAsync<MyData>(fs, cancellationToken);

// 設定ファイルが存在するか確認する
if (@"C:\config.json".IsConfigExists(fs))
{
    // ...
}
```

### CandlePatternFileStorage

ローソク足パターンストレージ:

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

var patternStorage = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);
```

### NativeIdStorage、SecurityMappingStorage、およびその他

ほとんどのマッピングおよび識別子ストレージも `IFileSystem` を受け取ります。

```csharp
// NativeIdStorage
var nativeIdStorage = new NativeIdStorage(Paths.FileSystem, Paths.SecurityNativeIdDir, executor);

// SecurityMappingStorage
var mappingStorage = new SecurityMappingStorage(Paths.FileSystem, Paths.SecurityMappingDir, executor);

// ExtendedInfoStorage
var extInfoStorage = new ExtendedInfoStorage(Paths.FileSystem, Paths.SecurityExtendedInfo, executor);
```

## 一般的な使用パターン

StockSharp アプリケーションでは、`IFileSystem` への参照を保持し、それをすべてのコンポーネントへ渡すことが推奨されます。

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// ファイルシステムを取得する
var fs = Paths.FileSystem;

// データストレージを作成する
var drive = new LocalMarketDataDrive(fs, Paths.StorageDir);

// コネクターを作成し、ストレージを設定する
var connector = new Connector();
connector.Adapter.StorageSettings.Drive = drive;

// ファイルから設定を読み込む
var configFile = Path.Combine(Paths.AppDataPath, "connector_config.json");

if (configFile.IsConfigExists(fs))
{
    var config = configFile.Deserialize<SettingsStorage>(fs);
    connector.Load(config);
}
```

## 非推奨のオーバーロード

多くのクラスは後方互換性のために `IFileSystem` なしのコンストラクターを保持していますが、それらには `[Obsolete]` 属性が付けられています。これらのコンストラクターは内部で `Paths.FileSystem` を使用します。

```csharp
// 非推奨の方法
[Obsolete("Use IFileSystem overload.")]
public LocalMarketDataDrive(string path)
    : this(Paths.FileSystem, path) { }

// 推奨される方法
public LocalMarketDataDrive(IFileSystem fileSystem, string path) { }
```

非推奨のコンストラクターは将来のバージョンで削除されるため、明示的に `IFileSystem` を渡すオーバーロードを常に使用することが推奨されます。
