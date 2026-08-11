# График Point and Figure

Графики Point & Figure полностью отказываются от времени и строят колонки из крестиков X (рост) и ноликов O (падение). Колонка продолжает расти, пока цена движется в её направлении на целые клетки (box); движение против неё на `reversal` клеток начинает новую колонку. Такой результат наглядно показывает поддержку, сопротивление и пробои.

## Живое демо

```chart-demo point-figure
```

## Настройка

Добавьте `PointFigureSeries` с параметрами `boxSize` и `reversal`, затем передайте в него сырые свечи — серия сама построит колонки:

```js
const series = chart.addSeries(SSChart.PointFigureSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
  reversal: 2,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

`boxSize` задаёт цену одной клетки X/O; `reversal` — это сколько клеток против текущей колонки нужно, чтобы начать новую (3 — классическое значение).

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Renko](renko.md)
- [Свечи](candlestick.md)
