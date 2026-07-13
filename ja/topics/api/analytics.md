# 分析スクリプト

[S#](../api.md) は、結果の可視化を伴う任意のマーケットデータ分析を実行できる分析スクリプトサブシステムを実装しています。クラスは `StockSharp.Algo.Analytics` 名前空間にあります。

## IAnalyticsScript — メインインターフェイス

[IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) インターフェイスは、単一のメソッドを定義します:

```cs
Task Run(
    ILogReceiver logs,
    IAnalyticsPanel panel,
    SecurityId[] securities,
    DateTime from,
    DateTime to,
    IStorageRegistry storage,
    IMarketDataDrive drive,
    StorageFormats format,
    DataType dataType,
    CancellationToken cancellationToken);
```

パラメーター:

- **logs** — 診断メッセージを出力するためのログ受信器。
- **panel** — 分析結果を表示するためのパネル。
- **securities** — 分析対象の銘柄配列。
- **from** / **to** — 時間範囲。
- **storage** — マーケットデータストレージレジストリ。
- **drive** — データソース。
- **format** — データ保存形式。
- **dataType** — 分析するデータの型。
- **cancellationToken** — キャンセレーショントークン。

## IAnalyticsPanel — 結果パネル

[IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) インターフェイスは、さまざまな可視化を作成するためのメソッドを提供します:

- **CreateGrid(params string[] columns)** — 指定された列を持つテーブル [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) を作成します。
- **CreateChart\<X, Y\>()** — 2 次元チャート [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) を作成します。
- **CreateChart\<X, Y, Z\>()** — 3 次元チャート [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`3) を作成します。
- **DrawHeatmap(string[] xTitles, string[] yTitles, double[,] data)** — ヒートマップを描画します。
- **Draw3D(string[] xTitles, string[] yTitles, data, xTitle, yTitle, zTitle)** — 3D 可視化を描画します。

## IAnalyticsChart — チャート

[IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) インターフェイスは、データ系列を追加するためのメソッドを提供します:

```cs
void Append(string title, IEnumerable<X> xValues, IEnumerable<Y> yValues,
    DrawStyles style, Color? color = null);
```

利用可能な描画スタイル ([DrawStyles](xref:StockSharp.Algo.Analytics.DrawStyles)):

- **Line** — 折れ線チャート。
- **DashedLine** — 破線。
- **Histogram** — ヒストグラム。
- **Bubble** — バブルチャート。

## IAnalyticsGrid — テーブル

[IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) インターフェイスにより、表形式データを表示できます:

- **SetSort(string column, bool ascending)** — 列によるソートを設定します。
- **SetRow(params object[] values)** — データ行を追加します。

## 組み込みスクリプト

`StockSharp.Algo.Analytics.CSharp` パッケージには、既製のスクリプトが含まれています:

- **IndicatorScript** — チャート上でインジケーターを計算して可視化します。
- **ChartDrawScript** — さまざまなチャートタイプの構築を実演します。
- **PriceVolumeScript** — 価格水準ごとの出来高分布を分析します。

## 例: カスタム分析スクリプト

以下は、銘柄一覧のローソク足を読み込み、終値を折れ線チャートに表示するスクリプト例です:

```cs
public class MyAnalyticsScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        // 2 次元チャートを作成
        var chart = panel.CreateChart<DateTime, decimal>();

        foreach (var secId in securities)
        {
            // ローソク足ストレージを取得
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            // 対象期間のデータを読み込む
            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
            {
                logs.AddWarningLog($"{secId} のデータがありません");
                continue;
            }

            // 系列をチャートに追加
            chart.Append(secId.ToString(),
                candles.Select(c => c.OpenTime.UtcDateTime),
                candles.Select(c => c.ClosePrice),
                DrawStyles.Line);

            logs.AddInfoLog($"{secId}: {candles.Length} 本のローソク足を読み込みました");
        }
    }
}
```

## 例: 出来高テーブル

```cs
public class VolumeTableScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        var grid = panel.CreateGrid("Instrument", "ローソク足合計",
            "出来高合計", "平均出来高");
        grid.SetSort("出来高合計", false);

        foreach (var secId in securities)
        {
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
                continue;

            var totalVolume = candles.Sum(c => c.TotalVolume);
            var avgVolume = totalVolume / candles.Length;

            grid.SetRow(secId.ToString(), candles.Length,
                totalVolume, avgVolume);
        }
    }
}
```

## 関連項目

[インジケーター](indicators.md)

[データストレージ](market_data_storage.md)
