# Band

Eine Band-Serie zeichnet eine obere und eine untere Begrenzungslinie und füllt den Kanal zwischen ihnen aus. Sie eignet sich von Natur aus für Umhüllungen (Envelopes) und Bollinger-Bänder oder für jede Studie, die einen Preiskorridor erzeugt.

## Live-Demo

```chart-demo band
```

## Einrichtung

Fügen Sie eine `BandSeries` hinzu und speisen Sie sie mit `{ time, upper, lower }`-Punkten:

```js
const band = chart.addSeries(SSChart.BandSeries, {
  upperColor: '#26a69a',
  lowerColor: '#ef5350',
  fillColor: 'rgba(74,158,255,0.10)',
});

band.setData(data.map(d => ({ time: d.time, upper: d.upper, lower: d.lower })));

chart.timeScale().fitContent();
```

Berechnen Sie `upper`/`lower` so, wie es die Studie erfordert — für Bollinger-Bänder bilden Sie einen gleitenden Durchschnitt des Schlusskurses und addieren bzw. subtrahieren ein Vielfaches seiner Standardabweichung. Legen Sie das Band über eine Linien- oder Kerzen-Serie, die den Preis selbst trägt.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Linie](line.md)
- [Volumenprofil](volume_profile.md)
