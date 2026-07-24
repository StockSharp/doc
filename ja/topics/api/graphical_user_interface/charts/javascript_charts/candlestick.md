# ローソク足

ローソク足はデフォルトの価格系列です。各バーは始値と終値の間を実体として描画し、高値と安値まではヒゲとして伸ばし、上昇か下落かで色分けされます。価格を読み取る最も情報密度の高い方法であり、ほとんどのチャートの出発点になります。

## ライブデモ

```chart-demo candlestick
```

## セットアップ

`CandlestickSeries` を追加し、そこに `{ time, open, high, low, close }` のポイントを与えます（time は Unix 秒です）。

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
});

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  borderUpColor: '#26a69a',
  borderDownColor: '#ef5350',
  wickUpColor: '#26a69a',
  wickDownColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

リアルタイムのバーをプッシュするには `series.update({ time, open, high, low, close })` を呼び出します。同じタイムスタンプは最後のローソク足を置き換え、より新しいタイムスタンプは新しいローソク足を追加します。

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [OHLC バー](bar.md)
- [Heikin-Ashi](heikin_ashi.md)
