# Renko

Renko charts are built from fixed-size price *bricks* and ignore time: a new brick is added only when price moves by the box size, so sideways noise collapses and trends stand out. Each brick spans one box in the direction of the move.

## Live demo

```chart-demo renko
```

## Setup

Add a `RenkoSeries` with a `boxSize` and feed it raw `{ time, open, high, low, close }` candles — the series builds the bricks itself:

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

Pick `boxSize` from the instrument's price range: too small produces noise, too large hides moves. A common rule is a fraction of the average bar range.

## See also

- [JavaScript charts](../javascript_charts.md)
- [Point and Figure](point_figure.md)
- [Heikin-Ashi](heikin_ashi.md)
