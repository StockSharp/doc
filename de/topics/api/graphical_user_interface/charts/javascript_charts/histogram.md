# Histogramm

Ein Histogramm zeichnet einen vertikalen Balken pro Punkt ausgehend von einem Basiswert. Am häufigsten wird es für das Volumen verwendet, wobei jeder Balken danach eingefärbt wird, ob die Kerze steigend oder fallend geschlossen hat, doch jede beliebige balkenweise Größe funktioniert.

## Live-Demo

```chart-demo histogram
```

## Einrichtung

Fügen Sie eine `HistogramSeries` hinzu und speisen Sie sie mit `{ time, value, color? }`-Punkten; eine punktweise `color` überschreibt die Farbe der Serie:

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

Um das Volumen unterhalb eines Kurscharts statt in einem eigenen Chart anzuzeigen, platzieren Sie das Histogramm auf einer Overlay-Kursskala und heften Sie es an den unteren Rand:

```js
const volume = chart.addSeries(SSChart.HistogramSeries, { priceScaleId: '', priceFormat: { type: 'volume' } });
volume.priceScale().applyOptions({ scaleMargins: { top: 0.82, bottom: 0 } });
```

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Fläche](area.md)
- [Kerzenchart](candlestick.md)
