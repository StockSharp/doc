# Renko

Графики Renko строятся из ценовых *кирпичей* фиксированного размера и игнорируют время: новый кирпич добавляется только тогда, когда цена сдвигается на размер бокса, поэтому боковой шум схлопывается, а тренды выделяются. Каждый кирпич занимает один бокс в направлении движения.

## Живое демо

```chart-demo renko
```

## Настройка

Добавьте `RenkoSeries` с параметром `boxSize` и передайте ему исходные свечи `{ time, open, high, low, close }` — серия сама построит кирпичи:

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

Выбирайте `boxSize` исходя из ценового диапазона инструмента: слишком маленький порождает шум, слишком большой скрывает движения. Распространённое правило — доля от среднего диапазона бара.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [График Point and Figure](point_figure.md)
- [Свечи Heikin-Ashi](heikin_ashi.md)
