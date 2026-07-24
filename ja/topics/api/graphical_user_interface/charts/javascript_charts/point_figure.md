# ポイント・アンド・フィギュア

ポイント・アンド・フィギュア（P&F）チャートは時間軸を完全に排し、X（上昇）とO（下落）の列を描画します。価格が丸ごと1ボックス分だけその方向に動き続ける限り列は伸び続け、逆方向に `reversal` 個分のボックスが積み上がると新しい列が始まります。これにより、サポート、レジスタンス、ブレイクアウトが際立ちます。

## ライブデモ

```chart-demo point-figure
```

## セットアップ

`boxSize` と `reversal` を指定した `PointFigureSeries` を追加し、生のローソク足を渡します。列はシリーズ自身が構築します。

```js
const series = chart.addSeries(SSChart.PointFigureSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
  reversal: 2,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

`boxSize` は X/O 1個あたりの価格幅を設定します。`reversal` は、新しい列を始めるために現在の列とは逆方向に必要なボックスの数です（3 が古典的な値です）。

## 関連情報

- [JavaScript チャート](../javascript_charts.md)
- [練行足（Renko）](renko.md)
- [ローソク足](candlestick.md)
