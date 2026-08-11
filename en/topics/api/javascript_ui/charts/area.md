# Area

An area series is a line with the region below it filled by a vertical gradient. It reads like a line chart but emphasizes the magnitude of a single value over time, which suits equity curves and price overviews.

## Live demo

```chart-demo area
```

## Setup

Add an `AreaSeries` and feed it `{ time, value }` points; `topColor`/`bottomColor` define the gradient:

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

## See also

- [JavaScript charts](../charts.md)
- [Line](line.md)
- [Histogram](histogram.md)
