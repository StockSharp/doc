# ライン

ラインシリーズは、バーごとの単一の値（通常は終値）を1本の連続した折れ線でつなぎます。トレンドを示したり、移動平均のような派生シリーズをプロットしたりするのに最もシンプルな方法です。

## ライブデモ

```chart-demo line
```

## セットアップ

`LineSeries` を追加し、`{ time, value }` のポイントを渡します。

```js
const series = chart.addSeries(SSChart.LineSeries, {
  color: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

単一の値を持つデータセットであれば何でも利用できます。`c.close` をインジケーターの値に置き換えれば、価格チャート上に重ねて表示できます。

## 関連項目

- [JavaScript チャート](../charts.md)
- [エリア](area.md)
- [バンド](band.md)
