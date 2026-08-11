# Область

Серия области — это линия с областью под ней, залитой вертикальным градиентом. Она читается как линейный график, но подчёркивает величину одного значения во времени, что подходит для кривых эквити и обзоров цены.

## Живое демо

```chart-demo area
```

## Настройка

Добавьте `AreaSeries` и передайте ей точки `{ time, value }`; `topColor`/`bottomColor` задают градиент:

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

## См. также

- [JavaScript-графики](../charts.md)
- [Линия](line.md)
- [Гистограмма](histogram.md)
