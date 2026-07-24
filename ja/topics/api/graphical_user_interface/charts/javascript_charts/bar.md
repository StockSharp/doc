# OHLCバー

OHLCバーは、ローソク足と同じ4つの価格を、塗りつぶした実体なしで表示します。縦の線が高値〜安値のレンジを表し、左側のティックが始値を、右側のティックが終値を示します。すべてのバーの始値と終値を表示しながらも、チャートを軽く保ちます。

## ライブデモ

```chart-demo bar
```

## セットアップ

`BarSeries` を追加し、`{ time, open, high, low, close }` のポイントを与えます。

```js
const series = chart.addSeries(SSChart.BarSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

バーは、終値が始値以上のときに上昇色で、そうでない場合は下降色で表示されます。

## 関連項目

- [JavaScriptチャート](../javascript_charts.md)
- [ローソク足](candlestick.md)
- [ライン](line.md)
