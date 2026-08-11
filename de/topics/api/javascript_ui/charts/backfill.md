# Nachladen der Historie

Ein Chart kann nicht die gesamte Historie eines Instruments auf einmal vorhalten, daher lädt er ein Fenster der jüngsten Bars und ruft ältere bei Bedarf ab. Der `ChartDataController` überwacht den sichtbaren Bereich und holt, sobald Sie in Richtung des linken Randes scrollen oder zoomen, die nächste Seite der Historie aus Ihrer Datenquelle und stellt sie vorne an.

## Live-Demo

Scrollen oder zoomen Sie in Richtung des linken Randes — ältere Bars werden seitenweise geladen (beachten Sie die Statusanzeige). Der Datenstrom hier hat eine künstliche Verzögerung, damit der Ladezustand sichtbar wird.

```chart-demo backfill
```

## Einrichtung

Übergeben Sie dem Controller den Chart, die von ihm gesteuerte Serie und eine Datenquelle. Die Datenquelle löst ein Symbol auf und liefert Seiten von Bars zurück; der Controller fordert ältere Seiten automatisch an:

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { ChartDataController } from '@stocksharp/chart/data';

// Eine Datenquelle: das Symbol auflösen, dann Seiten von Bars liefern, die vor `to` enden.
const dataSource = {
  resolveSymbol(request) {
    return Promise.resolve({ id: request.symbol, priceFormat: { type: 'price', precision: 2, minMove: 0.01 } });
  },
  getBars(request) {
    // request: { symbol, resolution, to?, countBack }. Gibt { bars, hasMoreBefore, hasMoreAfter } zurück.
    return fetchBars(request).then(bars => ({ bars, hasMoreBefore: bars.length > 0, hasMoreAfter: false }));
  },
  subscribeBars(request, listener) {
    // Echtzeit-Bars über den Rückruf senden (mit { bar, isFinal }); eine Abmeldefunktion zurückgeben.
    return () => {};
  },
};

const chart = createChart(document.getElementById('chart'), { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const controller = new ChartDataController({
  chart,
  series,
  dataSource,
  initialCount: 300,               // zuerst geladene Bars
  historyCount: 250,               // Bars pro älterer Seite
  historyPrefetchThreshold: 40,    // im Voraus laden, wenn weniger als 40 Bars vom linken Rand entfernt
  autoPrefetch: true,              // ältere Bars beim Scrollen / Zoomen automatisch laden
});

// Optional: Ladezustand und Fortschritt beobachten.
controller.subscribe(snap => {
  console.log(snap.loadedBars, snap.hasMoreBefore, snap.loadingHistory);
});

// Die erste Seite laden, dann den sichtbaren Bereich nahe dem rechten Rand platzieren, damit Platz zum Scrollen nach links bleibt.
controller.setSelection({ symbol: 'DEMO', resolution: '1h' }).then(() => {
  const loaded = controller.rawData().length;
  chart.timeScale().setVisibleLogicalRange({ from: Math.max(0, loaded - 90), to: loaded + 3 });
});
```

`historyPrefetchThreshold` gibt an, wie nah der sichtbare Bereich an den ältesten geladenen Bar heranreichen muss, bevor die nächste Seite abgerufen wird; `autoPrefetch` aktiviert dieses automatische Laden. Solange eine Seite unterwegs ist, ist `snapshot.loadingHistory` gleich `true`, sodass Sie einen Ladeindikator anzeigen können. Rufen Sie `controller.loadMoreBefore()` auf, um eine Seite manuell auszulösen, und `controller.dispose()`, um aufzuräumen.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Kerzenchart](candlestick.md)
- [Indikatoren](indicators.md)
