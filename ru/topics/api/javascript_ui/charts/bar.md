# OHLC-бары

OHLC-бары показывают те же четыре цены, что и свечи, но без закрашенного тела: вертикальная линия охватывает диапазон от максимума до минимума, левый тик отмечает цену открытия, а правый — цену закрытия. Они делают график лёгким, при этом по-прежнему показывая цену открытия и закрытия каждого бара.

## Живая демонстрация

```chart-demo bar
```

## Настройка

Добавьте `BarSeries` и передайте ему точки `{ time, open, high, low, close }`:

```js
const series = chart.addSeries(SSChart.BarSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

Бар окрашивается в цвет роста, когда закрытие находится на уровне открытия или выше него, и в цвет падения в остальных случаях.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Свечи](candlestick.md)
- [Линия](line.md)
