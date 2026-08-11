# Point and Figure

Point & Figure charts drop time entirely and plot columns of X's (rising) and O's (falling). A column keeps growing while price moves in its direction by whole boxes; a `reversal` number of boxes against it starts a new column. The result highlights support, resistance and breakouts.

## Live demo

```chart-demo point-figure
```

## Setup

Add a `PointFigureSeries` with a `boxSize` and a `reversal`, then feed it raw candles — the series builds the columns itself:

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

`boxSize` sets the price per X/O; `reversal` is how many boxes against the current column are needed to start a new one (3 is the classic value).

## See also

- [JavaScript charts](../charts.md)
- [Renko](renko.md)
- [Candlestick](candlestick.md)
