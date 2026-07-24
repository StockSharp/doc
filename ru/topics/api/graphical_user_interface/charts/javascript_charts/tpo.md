# TPO (Профиль рынка)

График TPO (Time Price Opportunity), также называемый Профилем рынка (Market Profile), показывает, как долго цена торговалась на каждом уровне в течение сессии, накапливая букву или блок для каждого временного интервала. Он выявляет область справедливой стоимости сессии, её точку контроля (point of control) и уровни, на которых цена провела мало времени (single prints).

## Живое демо

```chart-demo tpo
```

## Настройка

Добавьте `TpoSeries` и подавайте в неё OHLC-бары, каждый из которых несёт `sessionId`; серия сама строит распределение букв/блоков по сессиям:

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // режим отображения: Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` переключает между буквами и сплошными блоками (`Auto` выбирает по масштабу). Наложения — точка контроля (point of control), область стоимости (value area), начальный баланс (initial balance) и single prints — можно включать и выключать независимо друг от друга.

## См. также

- [JavaScript-графики](../javascript_charts.md)
- [Профиль объёма](volume_profile.md)
- [Footprint-график](footprint.md)
