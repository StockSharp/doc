# Свечи

Свечи — это ценовой ряд по умолчанию: каждый бар рисуется как тело между ценами открытия и закрытия с фитилями до максимума и минимума, окрашенное вверх или вниз. Это самый информационно насыщенный способ читать цену и отправная точка для большинства графиков.

## Живой пример

```chart-demo candlestick
```

## Настройка

Добавьте `CandlestickSeries` и подавайте в него точки `{ time, open, high, low, close }` (время в Unix-секундах):

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

Вызывайте `series.update({ time, open, high, low, close })`, чтобы отправить бар в реальном времени: та же метка времени заменяет последнюю свечу, более новая — добавляет новую.

## См. также

- [JavaScript-графики](../charts.md)
- [OHLC-бары](bar.md)
- [Свечи Heikin-Ashi](heikin_ashi.md)
