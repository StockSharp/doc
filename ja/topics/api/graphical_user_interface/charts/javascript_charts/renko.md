# 練行足 (Renko)

練行足チャートは固定サイズの価格*ブロック*から構築され、時間を無視します。価格がボックスサイズ分だけ動いたときにのみ新しいブロックが追加されるため、横ばい相場のノイズが抑えられ、トレンドが際立ちます。各ブロックは値動きの方向に1ボックス分だけ広がります。

## ライブデモ

```chart-demo renko
```

## セットアップ

`boxSize` を指定した `RenkoSeries` を追加し、生の `{ time, open, high, low, close }` ローソク足を渡します。シリーズが自身でブロックを構築します。

```js
const series = chart.addSeries(SSChart.RenkoSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

`boxSize` は銘柄の価格レンジから選びます。小さすぎるとノイズが生じ、大きすぎると値動きが隠れてしまいます。一般的な目安は、平均バーレンジの一定割合です。

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [ポイント・アンド・フィギュア](point_figure.md)
- [平均足 (Heikin-Ashi)](heikin_ashi.md)
