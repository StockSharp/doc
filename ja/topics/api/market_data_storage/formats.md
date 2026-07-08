# ストレージ形式

StockSharp は、[StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) 列挙体で定義される 2 つの市場データストレージ形式、**Binary** と **CSV** をサポートしています。各形式にはそれぞれ利点があり、異なるユースケースに適しています。

## Binary 形式

バイナリ形式は、StockSharp における主要な高性能データストレージ形式です。実装は `Algo/Storages/Binary/` 内の `BinaryMarketDataSerializer` クラスにあります。

### 機能

- **コンパクト性** -- データは圧縮付きのバイナリ形式でシリアライズされ、ファイルサイズを最小限に抑えます。
- **パフォーマンス** -- テキスト形式と比較して、読み取りと書き込みが大幅に高速です。
- **メタデータ** -- `BinaryMetaInfo` クラスは、始値と終値、端数値、タイムスタンプなどの補助情報を保存します。これにより、ファイル全体を読み込まずに一般的なデータ情報を素早く取得できます。
- **圧縮対応アーキテクチャ** -- この形式は、効率的なストリーミング圧縮を前提として最初から設計されています。

バイナリファイルの拡張子は `.bin` です。

### サポートされるデータ型

バイナリ形式は、主要なすべての市場データ型のシリアライズをサポートします。キャンドル、ティック（取引）、板情報（Level2）、Level1 データ、注文、自己約定です。各型には、その特定データの構造に最適化された専用シリアライザーがあります。

## CSV 形式

CSV（Comma-Separated Values）テキスト形式は、`Algo/Storages/Csv/` にある `CsvMarketDataSerializer` クラスで実装されています。

### 機能

- **可読性** -- ファイルは任意のテキストエディターや Excel で開くことができ、視覚的なデータ分析に使用できます。
- **編集可能性** -- 必要に応じてデータを手動で修正できます。
- **メタデータ** -- `CsvMetaInfo` クラスは、エンコーディング対応のメタデータ保存を提供します。これには `IncrementalOnly` プロパティ（差分データサポート）と `LastId`（最終レコード識別子）が含まれます。
- **より大きなファイルサイズ** -- テキスト表現は、ディスク容量を大幅に多く消費します。
- **より遅い処理** -- テキストデータの解析には追加の計算リソースが必要です。

CSV ファイルの拡張子は `.csv` です。

## どの形式をいつ使用するか

| シナリオ | 推奨形式 |
|----------|-------------------|
| 本番利用 | Binary |
| 大容量データ | Binary |
| パフォーマンス重視 | Binary |
| デバッグと診断 | CSV |
| 視覚的なデータ分析 | CSV |
| 外部ツールとの連携 | CSV |
| データの手動修正 | CSV |

## ディスク上のファイル構成

ディスク上のデータは、次のパス構造に従って整理されます。

```
{root_folder}/{first_letter}/{instrument_identifier}/{yyyy_MM_dd}/{file_name}.{extension}
```

拡張子は、バイナリ形式では `.bin`、テキスト形式では `.csv` です。この階層構成により、銘柄と日付による高速なデータ検索が可能になります。

たとえば、2024 年 4 月 1 日の AAPL@NASDAQ 銘柄の 5 分足キャンドルをバイナリ形式で保存する場合、次のようなパスに配置されます。

```
Storage/S/AAPL@NASDAQ/2024_04_01/candles_5m.bin
```

## 形式間の変換

StockSharp では、ある形式でデータを読み込み、別の形式で保存できます。これは、たとえば外部ツールで分析するためにバイナリデータを CSV にエクスポートする場合に便利です。

```cs
// バイナリストレージから読み込みます
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Binary);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();

// CSV ストレージに保存します
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Csv);
await csvStorage.SaveAsync(candles);
```

## コード例

形式は、[StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) 経由でストレージを作成するときに選択します。

```cs
var storageRegistry = new StorageRegistry();

// バイナリ形式でキャンドルストレージを作成します
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Binary);

// CSV 形式でキャンドルストレージを作成します
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Csv);

// データを読み込みます
var from = new DateTime(2024, 1, 1);
var to = new DateTime(2024, 1, 31);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();
```

ストレージ形式は、ティックデータや板情報を扱う場合にも指定できます。

```cs
// バイナリ形式のティック
var tickStorage = storageRegistry.GetTickMessageStorage(
    securityId, StorageFormats.Binary);

// CSV 形式の板情報
var depthStorage = storageRegistry.GetQuoteMessageStorage(
    securityId, StorageFormats.Csv);
```

## 関連項目

- [API の操作](api.md)
- [ストレージドライブ](drives.md)

