# TPO (Market Profile)

Ein TPO-Chart (Time Price Opportunity), auch Market Profile genannt, zeigt, wie lange der Preis während einer Session auf jedem Niveau gehandelt wurde, indem für jedes Zeitintervall ein Buchstabe oder Block gestapelt wird. Es macht die Fair-Value-Zone der Session sichtbar, ihren Point of Control sowie die Bereiche, in denen der Preis nur wenig Zeit verbrachte (Single Prints).

## Live-Demo

```chart-demo tpo
```

## Einrichtung

Fügen Sie eine `TpoSeries` hinzu und speisen Sie ihr OHLC-Bars ein, die jeweils eine `sessionId` tragen; die Series erstellt die Buchstaben-/Blockverteilung pro Session selbst:

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` schaltet zwischen Buchstaben und massiven Blöcken um (`Auto` wählt anhand des Zoom-Levels). Die Overlays — Point of Control, Value Area, Initial Balance und Single Prints — lassen sich jeweils unabhängig voneinander ein- und ausschalten.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Volume Profile](volume_profile.md)
- [Footprint](footprint.md)
