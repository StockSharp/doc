# Heikin-Ashi

Свечи Heikin-Ashi («усреднённый бар») рассчитываются из исходных значений OHLC для сглаживания шума: идущие подряд свечи одного цвета облегчают чтение тренда ценой сокрытия истинных цен открытия и закрытия. Они отрисовываются обычной свечной серией, которой передаются преобразованные значения.

## Живой пример

```chart-demo heikin-ashi
```

## Настройка

Рассчитайте значения Heikin-Ashi и передайте их в `CandlestickSeries`:

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

## См. также

- [Графики на JavaScript](../javascript_charts.md)
- [Свечи](candlestick.md)
- [Renko](renko.md)
