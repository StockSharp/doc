# Band

A band series draws an upper and a lower boundary line with the channel between them filled. It is the natural fit for envelopes and Bollinger Bands, or any study that produces a price corridor.

## Live demo

```chart-demo band
```

## Setup

Add a `BandSeries` and feed it `{ time, upper, lower }` points:

```js
const band = chart.addSeries(SSChart.BandSeries, {
  upperColor: '#26a69a',
  lowerColor: '#ef5350',
  fillColor: 'rgba(74,158,255,0.10)',
});

band.setData(data.map(d => ({ time: d.time, upper: d.upper, lower: d.lower })));

chart.timeScale().fitContent();
```

Compute `upper`/`lower` however the study requires — for Bollinger Bands, take a moving average of the close and add/subtract a multiple of its standard deviation. Overlay the band on a line or candlestick series that carries the price itself.

## See also

- [JavaScript charts](../javascript_charts.md)
- [Line](line.md)
- [Volume profile](volume_profile.md)
