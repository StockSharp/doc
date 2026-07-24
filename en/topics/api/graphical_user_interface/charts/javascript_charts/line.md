# Line

A line series joins a single value per bar — usually the close — into one continuous polyline. It is the cleanest way to show a trend or to plot a derived series such as a moving average.

## Live demo

```chart-demo line
```

## Setup

Add a `LineSeries` and feed it `{ time, value }` points:

```js
const series = chart.addSeries(SSChart.LineSeries, {
  color: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

Any single-value dataset works here — swap `c.close` for an indicator value to overlay it on the price chart.

## See also

- [JavaScript charts](../javascript_charts.md)
- [Area](area.md)
- [Band](band.md)
