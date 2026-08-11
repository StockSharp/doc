# Volumenprofil

Ein Volumenprofil aggregiert das gehandelte Volumen nach Preis und zeichnet es als horizontales Histogramm, wobei der Point of Control (der meistgehandelte Preis) und die Value Area markiert werden. Es beantwortet die Frage "wo wurde gehandelt", unabhängig davon, wann.

## Live-Demo

Das Profil wird über die jeweils sichtbaren Bars neu berechnet — scrollen und zoomen Sie, um zu beobachten, wie es sich verändert.

```chart-demo volume-profile
```

![Volumenprofil mit Point of Control und Value Area](../../../../images/chart_volume_profile.png)

## Einrichtung

Fügen Sie eine `ExactVolumeProfileSeries` hinzu (üblicherweise über einer Candlestick-Serie zur Einordnung) und speisen Sie sie mit denselben Order-Flow-Bars, die auch das Footprint verwendet:

```js
const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350', borderVisible: false,
});
candles.setData(exactBars);

const profile = chart.addSeries(SSChart.ExactVolumeProfileSeries, {
  tickSize: 0.25,
  rangeMode: SSChart.VolumeProfileRangeMode.Visible,     // Bereichsmodus: Visible | Fixed | Session
  displayMode: SSChart.VolumeProfileDisplayMode.BidAsk,  // Anzeigemodus: Total | BidAsk | Delta
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showLabels: true,
  profileWidth: 0.32,
});
profile.setData(exactBars);

chart.timeScale().fitContent();
```

`rangeMode` legt fest, was das Profil abdeckt: `Visible` berechnet über den sichtbaren Bereich neu, `Fixed` fixiert einen Bereich, `Session` erstellt ein Profil pro Session.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Footprint](footprint.md)
- [TPO (Market Profile)](tpo.md)
