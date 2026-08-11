# 平均足 (Heikin-Ashi)

Heikin-Ashi（「平均足」）ローソク足は、ノイズを平滑化するために生の OHLC から計算されます。同じ色のローソク足が連続することでトレンドが読み取りやすくなる一方、真の始値と終値は隠されます。変換した値を通常のローソク足シリーズに供給して描画します。

## ライブデモ

```chart-demo heikin-ashi
```

## セットアップ

Heikin-Ashi の値を計算し、それを `CandlestickSeries` に供給します。

```js
function heikinAshi(candles) {
  const out = [];
  let prevOpen = candles[0].open, prevClose = candles[0].close;
  for (const c of candles) {
    const haClose = (c.open + c.high + c.low + c.close) / 4;
    const haOpen = (prevOpen + prevClose) / 2;
    out.push({
      time: c.time,
      open: haOpen,
      high: Math.max(c.high, haOpen, haClose),
      low: Math.min(c.low, haOpen, haClose),
      close: haClose,
    });
    prevOpen = haOpen; prevClose = haClose;
  }
  return out;
}

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350',
});
series.setData(heikinAshi(candles));

chart.timeScale().fitContent();
```

## 関連項目

- [JavaScript チャート](../charts.md)
- [ローソク足](candlestick.md)
- [練行足 (Renko)](renko.md)
