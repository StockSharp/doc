# JavaScript-Diagramme

[StockSharp-JavaScript-Handelsdiagramme](https://github.com/StockSharp/Charts) ist eine eigenständige Diagrammbibliothek für Browser. Sie enthält die laufzeitabhängigkeitsfreie Canvas-Engine `sschart` und den Diagramm-Stack des StockSharp-Webterminals. Eine funktionsfähige Version steht als [Live-Demo](https://stocksharp.github.io/Charts/demo/) bereit.

![StockSharp-JavaScript-Handelsdiagramm](../../../../images/javascript_charts.jpg)

Im Gegensatz zu den Windows-Komponenten aus `StockSharp.Xaml.Charting` läuft diese Bibliothek im Browser und zeichnet direkt auf ein HTML-`canvas`. Die Engine wird über das globale Objekt `SSChart` bereitgestellt und kann in einem TypeScript-Build auch aus `src/sschart.ts` importiert werden.

## Funktionen

- Kerzen, OHLC-Balken, Linien, Flächen, Histogramme, Renko, Point and Figure, Volumenprofile, Cluster und Box-Serien.
- Laden historischer Daten und Echtzeitaktualisierungen mit `setData` und `update`.
- Handelsmarker, Preislinien, Fadenkreuz, Zoomen, Scrollen und automatische Bereichsberechnung.
- Eine Indikator-Engine mit etwa 160 Berechnungsimplementierungen.
- Überlagerte Indikatoren, synchronisierte Oszillatorbereiche und eine vom Fadenkreuz gesteuerte Legende.
- Helle und dunkle Designs, Kontextmenü, Indikatordialog und Umschaltung des Diagrammtyps.

## Diagramm zu einer Seite hinzufügen

Der Build erzeugt `dist/sschart.js`, das `window.SSChart` veröffentlicht. Zeitwerte werden der API als Unix-Zeitstempel in Sekunden übergeben.

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

Erstellen Sie ein Diagramm, fügen Sie eine Serie hinzu und laden Sie die Daten:

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
  crosshair: { mode: SSChart.CrosshairMode.Normal },
});

const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#00c853',
  downColor: '#ff3d57',
  borderVisible: false,
});

candles.setData([
  { time: 1704153600, open: 120, high: 122, low: 119, close: 121 },
  { time: 1704240000, open: 121, high: 124, low: 120, close: 123 },
]);

candles.update({
  time: 1704326400,
  open: 123,
  high: 123.5,
  low: 122.8,
  close: 123.2,
});

chart.timeScale().fitContent();
```

Ein Aufruf von `update` mit dem aktuellen Zeitstempel ersetzt den letzten Punkt. Ein neuerer Zeitstempel fügt einen Punkt hinzu.

## Vollständiger Terminal-Diagramm-Stack

Die Module unter `src/chart` erweitern die Basis-Engine um Terminalfunktionen:

- `IndicatorEngine`, Indikator-Renderer, Einstellungen und der Berechnungskatalog.
- Eine Diagrammtyp-Umschaltung für Kerzen, Balken, Linien, Flächen, Heikin-Ashi, Renko, Point and Figure, Cluster und Boxen.
- Legende, synchronisierte Nebenbereiche, Kontextmenü und Dialog zur Indikatorauswahl.
- Neuberechnung aktiver Indikatoren bei Änderungen der Echtzeitdaten.

Verwenden Sie `src/chart/app.ts` als Integrationsbeispiel für den vollständigen Stack.

## Aus dem Quellcode erstellen

Klonen Sie das Repository und verwenden Sie die enthaltenen npm-Skripte:

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

Der Entwicklungsserver stellt die Demo unter `http://localhost:8791/demo/index.html` bereit.

## Siehe auch

- [Charts-Repository](https://github.com/StockSharp/Charts)
- [Live-Demo](https://stocksharp.github.io/Charts/demo/)
- [Windows-Diagrammkomponenten](../charts.md)
