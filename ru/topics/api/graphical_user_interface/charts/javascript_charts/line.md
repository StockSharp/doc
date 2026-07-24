# Линия

Линейная серия соединяет одно значение на бар — обычно цену закрытия — в одну непрерывную ломаную линию. Это самый наглядный способ показать тренд или отобразить производную серию, например скользящую среднюю.

## Живой пример

```chart-demo line
```

## Настройка

Добавьте `LineSeries` и передайте ему точки `{ time, value }`:

```js
const series = chart.addSeries(SSChart.LineSeries, {
  color: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

Здесь подойдёт любой набор данных с единственным значением — замените `c.close` на значение индикатора, чтобы наложить его на ценовой график.

## Смотрите также

- [JavaScript-графики](../javascript_charts.md)
- [Область](area.md)
- [Полоса (Band)](band.md)
