# Профиль объёма

Профиль объёма агрегирует торгованный объём по цене и рисует его в виде горизонтальной гистограммы, отмечая point of control (цену с наибольшим объёмом торгов) и зону стоимости (value area). Он отвечает на вопрос «где совершались сделки», независимо от того, когда это происходило.

## Живая демонстрация

Профиль пересчитывается по тем барам, что находятся в поле зрения, — прокручивайте и масштабируйте, чтобы наблюдать за его изменением.

```chart-demo volume-profile
```

![Профиль объёма с point of control и зоной стоимости](../../../../../images/chart_volume_profile.png)

## Настройка

Добавьте `ExactVolumeProfileSeries` (обычно поверх свечной серии для контекста) и передайте ему те же точные бары потока ордеров, что используются footprint-графиком:

```js
const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350', borderVisible: false,
});
candles.setData(exactBars);

const profile = chart.addSeries(SSChart.ExactVolumeProfileSeries, {
  tickSize: 0.25,
  rangeMode: SSChart.VolumeProfileRangeMode.Visible,     // Visible | Fixed | Session
  displayMode: SSChart.VolumeProfileDisplayMode.BidAsk,  // Total | BidAsk | Delta
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showLabels: true,
  profileWidth: 0.32,
});
profile.setData(exactBars);

chart.timeScale().fitContent();
```

`rangeMode` задаёт, что охватывает профиль: `Visible` пересчитывает по видимой области, `Fixed` закрепляет один диапазон, `Session` строит отдельный профиль на каждую сессию.

## Смотрите также

- [JavaScript-графики](../javascript_charts.md)
- [Footprint](footprint.md)
- [TPO (Market profile)](tpo.md)
