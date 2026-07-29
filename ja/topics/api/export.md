# データエクスポート

[S#](../api.md) は、さまざまな形式をサポートするマーケットデータエクスポートサブシステムを実装しています。すべてのエクスポーターは基底クラス [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) から継承され、統一された非同期インターフェイスをサポートします。

## BaseExporter

抽象基底クラス [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) は、すべてのエクスポーターに共通する契約を定義します。

- `DataType` - エクスポートされるデータの種類（ティック、ローソク足、オーダーブックなど）。
- `Encoding` - エンコーディング（既定では UTF-8）。
- **Export\<T\>(IAsyncEnumerable\<T\>, CancellationToken)** - メインのエクスポートメソッド。`Task<(int count, DateTime? lastTime)>` を返します。これはエクスポートされたレコード数と最後のレコードの時刻です。

このメソッドは、データを型固有のハンドラーへ自動的にルーティングします。対象は [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage)、[Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage)、[ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)（ティック、注文ログ、トランザクション）、[CandleMessage](xref:StockSharp.Messages.CandleMessage)、[NewsMessage](xref:StockSharp.Messages.NewsMessage)、[SecurityMessage](xref:StockSharp.Messages.SecurityMessage)、[PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage)、[IndicatorValue](xref:StockSharp.Messages.IndicatorValue)、[BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage) です。

## エクスポーターの種類

### 1. TextExporter - CSV/Text エクスポート

[TextExporter](xref:StockSharp.Algo.Export.TextExporter) は、SmartFormat テンプレートを使用してデータをテキスト形式へエクスポートします。

- **コンストラクター**: `(DataType dataType, Stream stream, string template, string header)`
- テンプレートは SmartFormat 構文を使用します。例: `{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}`

```cs
await using var stream = File.Create("trades.csv");
var exporter = new TextExporter(DataType.Ticks, stream,
    "{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}",
    "日付;価格;出来高");

var (count, lastTime) = await exporter.Export(tickMessages, token);
```

### 2. JsonExporter - JSON エクスポート

[JsonExporter](xref:StockSharp.Algo.Export.JsonExporter) はデータを JSON 形式で保存します。

- **コンストラクター**: `(DataType dataType, Stream stream)`
- `Indent` - インデント付き整形（既定値は `true`）。

```cs
await using var stream = File.Create("candles.json");
var exporter = new JsonExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 3. XmlExporter - XML エクスポート

[XmlExporter](xref:StockSharp.Algo.Export.XmlExporter) はデータを XML 形式で保存します。

- **コンストラクター**: `(DataType dataType, Stream stream)`
- `Indent` - インデント付き整形（既定値は `true`）。

```cs
await using var stream = File.Create("candles.xml");
var exporter = new XmlExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 4. ExcelExporter - Excel エクスポート

[ExcelExporter](xref:StockSharp.Algo.Export.ExcelExporter) はデータを Excel スプレッドシートへエクスポートします。

- **コンストラクター**: `(IExcelWorkerProvider provider, DataType dataType, Stream stream, Action breaked)`
- 最大行数: 1,048,576（Excel 形式の制限）。

```cs
await using var stream = File.Create("data.xlsx");
var exporter = new ExcelExporter(excelProvider, DataType.Ticks, stream,
    () => { /* 中断を処理 */ });
await exporter.Export(tickMessages, token);
```

### 5. DatabaseExporter - データベースエクスポート

[DatabaseExporter](xref:StockSharp.Algo.Export.DatabaseExporter) は、LinqToDB 経由でデータをデータベースに保存します。

- **コンストラクター**: `(IDatabaseProvider dbProvider, DataType dataType, DatabaseConnectionPair connection, decimal? priceStep, decimal? volumeStep)`
- `BatchSize` - レコードのバッチサイズ（既定値は 50）。
- **CheckUnique** - レコードの一意性をチェックします（既定値は `false`）。
- **DropExisting** - エクスポート前に既存データを削除します（既定値は `false`）。

```cs
var exporter = new DatabaseExporter(
    dbProvider, DataType.Ticks, dbConnection)
{
    BatchSize = 100,
    CheckUnique = true
};
await exporter.Export(tickMessages, token);
```

### 6. StockSharpExporter - StockSharp ネイティブ形式

[StockSharpExporter](xref:StockSharp.Algo.Export.StockSharpExporter) は、内部 StockSharp ストレージ形式でデータを保存します。

- **コンストラクター**: `(DataType dataType, IStorageRegistry storageRegistry, IMarketDataDrive drive, StorageFormats format)`
- `BatchSize` - レコードのバッチサイズ（既定値は 50）。

```cs
var exporter = new StockSharpExporter(
    DataType.Ticks, storageRegistry, drive, StorageFormats.Binary);
await exporter.Export(tickMessages, token);
```

## TemplateTxtRegistry - テキストテンプレートレジストリ

[TemplateTxtRegistry](xref:StockSharp.Algo.Export.TemplateTxtRegistry) クラスには、[TextExporter](xref:StockSharp.Algo.Export.TextExporter) 経由でさまざまなデータ型をエクスポートするための定義済みテンプレートが含まれています。

- **TemplateTxtTick** - ティックデータ用テンプレート。
- **TemplateTxtDepth** - オーダーブック用テンプレート。
- **TemplateTxtCandle** - ローソク足用テンプレート。
- **TemplateTxtLevel1** - Level1 データ用テンプレート。
- **TemplateTxtOrderLog** - 注文ログ用テンプレート。
- **TemplateTxtTransaction** - トランザクション用テンプレート。
- **TemplateTxtSecurity** - 銘柄用テンプレート。
- **TemplateTxtNews** - ニュース用テンプレート。

テンプレートは必要に応じてカスタマイズまたは置換できます。このレジストリは [IPersistable](xref:Ecng.Serialization.IPersistable) を実装し、設定から保存および読み込みできます。

## 関連項目

[データインポート](import.md)

[データストレージ](market_data_storage.md)
