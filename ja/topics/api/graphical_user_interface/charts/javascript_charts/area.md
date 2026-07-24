# エリア

エリア系列は、その下の領域を垂直方向のグラデーションで塗りつぶしたラインです。ラインチャートのように読めますが、時間経過に伴う単一の値の大きさを強調するため、資産曲線（エクイティカーブ）や価格の概観に適しています。

## ライブデモ

```chart-demo area
```

## セットアップ

`AreaSeries` を追加し、`{ time, value }` のポイントを与えます。`topColor`/`bottomColor` がグラデーションを定義します。

```js
const series = chart.addSeries(SSChart.AreaSeries, {
  topColor: 'rgba(74,158,255,0.3)',
  bottomColor: 'rgba(74,158,255,0.02)',
  lineColor: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [ライン](line.md)
- [ヒストグラム](histogram.md)
