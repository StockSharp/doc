# Footprint-график

Footprint-график раскрывает каждый бар, показывая объём, торгованный на каждой цене внутри него, с разбивкой на bid и ask. Раскраска по дисбалансу подсвечивает, где доминировали агрессивные покупатели или продавцы, — это основа чтения потока ордеров (order-flow).

## Живая демонстрация

Приблизьте, чтобы разобрать отдельные ячейки, — footprint читаем только на нескольких барах за раз.

```chart-demo footprint
```

![Footprint-график](../../../../images/chart_footprint.png)

## Настройка

Добавьте `FootprintSeries` и передайте ему точные бары потока ордеров. Каждый бар несёт `dataMode: 'exact'`, свои значения OHLC и массив `levels` из `{ price, bidVolume, askVolume, tradeCount }`:

```js
const series = chart.addSeries(SSChart.FootprintSeries, {
  tickSize: 0.25,
  mode: SSChart.FootprintDisplayMode.BidAsk,       // режим отображения: BidAsk | Delta | Total | Ladder
  detailLevel: SSChart.FootprintDetailLevel.Auto,  // уровень детализации: Auto | Numbers | Heatmap | Summary
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showUnfinishedAuctions: true,
});

series.setData(exactBars);
```

`mode` определяет, что показывает каждая ячейка (bid × ask, дельта, суммарный объём или лестница); `detailLevel` меняет числовую детализацию на плотность по мере зума — `Auto` переключает автоматически.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Профиль объёма](volume_profile.md)
- [TPO (Профиль рынка)](tpo.md)
