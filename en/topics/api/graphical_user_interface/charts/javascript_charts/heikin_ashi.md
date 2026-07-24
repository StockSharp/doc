# Heikin-Ashi

Heikin-Ashi ("average bar") candles are computed from the raw OHLC to smooth out noise: consecutive candles of the same color make trends easier to read at the cost of hiding the true open and close. They are drawn with the ordinary candlestick series fed transformed values.

## Live demo

```chart-demo heikin-ashi
```

## Setup

Compute the Heikin-Ashi values and feed them to a `CandlestickSeries`:

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

## See also

- [JavaScript charts](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Renko](renko.md)
