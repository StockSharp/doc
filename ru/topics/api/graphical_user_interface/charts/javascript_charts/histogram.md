# Гистограмма

Гистограмма рисует по каждой точке вертикальный бар от базового значения. Чаще всего её используют для объёма, где каждый бар окрашивается в зависимости от того, закрылась свеча вверх или вниз, но подойдёт любая величина в разрезе бара.

## Живой пример

```chart-demo histogram
```

## Настройка

Добавьте `HistogramSeries` и передайте ей точки `{ time, value, color? }`; заданный для точки `color` переопределяет цвет серии:

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

Чтобы показать объём под ценовым графиком, а не отдельно, разместите гистограмму на наложенной ценовой шкале и прижмите её к низу:

```js
const volume = chart.addSeries(SSChart.HistogramSeries, { priceScaleId: '', priceFormat: { type: 'volume' } });
volume.priceScale().applyOptions({ scaleMargins: { top: 0.82, bottom: 0 } });
```

## Смотрите также

- [JavaScript-графики](../javascript_charts.md)
- [Область](area.md)
- [Свечи](candlestick.md)
