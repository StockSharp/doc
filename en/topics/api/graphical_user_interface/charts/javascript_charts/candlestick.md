# Candlestick

Candlesticks are the default price series: each bar is drawn as a body between the open and close with wicks to the high and low, colored up or down. They are the most information-dense way to read price and the starting point for most charts.

## Live demo

```chart-demo candlestick
```

## Setup

Add a `CandlestickSeries` and feed it `{ time, open, high, low, close }` points (time is Unix seconds):

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

Call `series.update({ time, open, high, low, close })` to push a real-time bar: the same timestamp replaces the last candle, a newer timestamp appends a new one.

## See also

- [JavaScript charts](../javascript_charts.md)
- [OHLC bars](bar.md)
- [Heikin-Ashi](heikin_ashi.md)
