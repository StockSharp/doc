# データインポート

[S#](../api.md) は、CSV ファイルからマーケットデータをインポートするサブシステムを実装しています。主なクラスは `StockSharp.Algo.Import` 名前空間に配置されています。

## CsvParser — 基本パーサー

[CsvParser](xref:StockSharp.Algo.Import.CsvParser) クラスは CSV ファイルを解析し、行を [S#](../api.md) メッセージに変換します。

- **コンストラクター**: `(DataType dataType, IEnumerable<FieldMapping> fields)`
- **列区切り** — 列区切り文字（既定値 `","`）。
- **行区切り** — 行区切り文字（既定値 CRLF）。
- **ヘッダーのスキップ行数** — ファイル先頭からスキップする行数（既定値 `0`）。
- **識別子のない銘柄を無視** — 認識できない銘柄を含む行を無視するかどうか（既定値 `true`）。
- **Parse(Stream)** — 解析メソッド。`IAsyncEnumerable<Message>` を返します。

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);
var parser = new CsvParser(DataType.Ticks, fields)
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");

await foreach (var msg in parser.Parse(stream))
{
    // 各メッセージを処理します
}
```

## CsvImporter — ストレージ保存を伴うインポート

[CsvImporter](xref:StockSharp.Algo.Import.CsvImporter) クラスは [CsvParser](xref:StockSharp.Algo.Import.CsvParser) を拡張し、データをマーケットデータストレージへ自動保存する機能を追加します。

- **コンストラクター**: `(DataType dataType, IEnumerable<FieldMapping> fields, ISecurityStorage securityStorage, IExchangeInfoProvider exchangeInfoProvider, Func<SecurityId, IMarketDataStorage> getStorage)`
- **Import(Stream, Action\<int\> progress, CancellationToken)** — インポートを実行し、`ValueTask<(int count, DateTime? lastTime)>` を返します。
- **重複銘柄を更新** — 重複する銘柄を更新するかどうか（既定値 `false`）。
- **銘柄更新** — 銘柄が更新されたときに発生するイベント。

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);

var importer = new CsvImporter(
    DataType.Ticks,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, DataType.Ticks))
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");
var (count, lastTime) = await importer.Import(
    stream,
    p => Console.WriteLine($"進捗: {p}%"),
    token);

Console.WriteLine($"{count} 件のレコードをインポートしました。最後: {lastTime}");
```

## FieldMapping — フィールド記述

[FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) クラスは、CSV ファイルの列とメッセージプロパティの間のマッピングを記述します。

主なプロパティ:

- **名前** — メッセージ内のフィールド名。
- **表示名** — 表示名。
- **型** — 値の型。
- **順序** — ファイル内の列インデックス（0 から開始）。
- **必須** — フィールドが必須かどうか。
- **形式** — 解析形式（例: 日付形式）。
- **既定値** — 既定値。
- **ゼロを null として扱う** — ゼロ値を `null` として解釈するかどうか。

カスタムの値変換には [FieldMappingValue](xref:StockSharp.Algo.Import.FieldMappingValue) を使用します。たとえば、テキスト値から列挙値へのマッピングを定義できます。

```cs
var sideField = fields.First(f => f.Name == "Side");
sideField.Order = 3;
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "B",
    ValueTo = Sides.Buy
});
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "S",
    ValueTo = Sides.Sell
});
```

## FieldMappingRegistry — 標準フィールドレジストリ

静的クラス [FieldMappingRegistry](xref:StockSharp.Algo.Import.FieldMappingRegistry) は、標準的なフィールドセットを作成するメソッドを提供します。

- **CreateFields(DataType)** — 指定されたデータ型に対応する [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) のリストを返します。

サポートされるデータ型: ティック、ローソク足、板、Level1、注文ログ、トランザクション、銘柄、ニュース、ポジション。

## ImportSettings — インポート設定

[ImportSettings](xref:StockSharp.Algo.Import.ImportSettings) クラスは、すべてのインポートパラメーターを 1 つの構成オブジェクトにまとめます。

- **データ型** — インポートするデータの型。
- **ファイル名** — ファイルパス。
- **ディレクトリ** — ファイル検索用ディレクトリ。
- **ファイルマスク** — ファイル検索マスク（例: `*.csv`）。
- **列区切り** — 列区切り文字。
- **ヘッダーのスキップ行数** — スキップする行数。
- **選択したフィールド** — インポート対象として選択されたフィールド。
- **重複銘柄を更新** — 重複する銘柄を更新するかどうか。

ヘルパーメソッド:

- **GetFiles(IFileSystem)** — マスクに一致するファイルのリストを取得します。
- **FillParser(CsvParser)** — パーサー設定を設定します。
- **FillImporter(CsvImporter)** — インポーター設定を設定します。

```cs
var settings = new ImportSettings
{
    DataType = DataType.Ticks,
    FileName = "trades.csv",
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

var fields = FieldMappingRegistry.CreateFields(settings.DataType);
// 列順を設定します
fields[0].Order = 0; // SecurityId
fields[1].Order = 1; // Date
fields[2].Order = 2; // Price
fields[3].Order = 3; // Volume

var importer = new CsvImporter(
    settings.DataType,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, settings.DataType));

settings.FillImporter(importer);

await using var stream = File.OpenRead(settings.FileName);
var (count, lastTime) = await importer.Import(stream, p => { }, token);
```

## 関連項目

[データエクスポート](export.md)

[データストレージ](market_data_storage.md)
