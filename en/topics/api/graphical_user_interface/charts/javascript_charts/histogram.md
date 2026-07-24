# Histogram

A histogram draws a vertical bar per point from a base value. Its most common use is volume, where each bar is colored by whether the candle closed up or down, but any per-bar quantity works.

## Live demo

```chart-demo histogram
```

## Setup

Add a `HistogramSeries` and feed it `{ time, value, color? }` points; a per-point `color` overrides the series color:

```js
const series = chart.addSeries(SSChart.HistogramSeries, {
  priceFormat: { type: 'volume' },
});

series.setData(candles.map(c => ({
  time: c.time,
  value: c.volume,
  color: c.close >= c.open ? 'rgba(38,166,154,0.7)' : 'rgba(239,83,80,0.7)',
})));

chart.timeScale().fitContent();
```

To show volume underneath a price chart instead of on its own, put the histogram on an overlay price scale and pin it to the bottom:

```js
const volume = chart.addSeries(SSChart.HistogramSeries, { priceScaleId: '', priceFormat: { type: 'volume' } });
volume.priceScale().applyOptions({ scaleMargins: { top: 0.82, bottom: 0 } });
```

## See also

- [JavaScript charts](../javascript_charts.md)
- [Area](area.md)
- [Candlestick](candlestick.md)
