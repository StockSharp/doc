# Nachladen der Historie

Ein Chart kann nicht die gesamte Historie eines Instruments auf einmal vorhalten, daher lädt er ein Fenster der jüngsten Bars und ruft ältere bei Bedarf ab. Der `ChartDataController` überwacht den sichtbaren Bereich und holt, sobald Sie in Richtung des linken Randes scrollen oder zoomen, die nächste Seite der Historie aus Ihrer Datenquelle und stellt sie vorne an.

## Live-Demo

Scrollen oder zoomen Sie in Richtung des linken Randes — ältere Bars werden seitenweise geladen (beachten Sie die Statusanzeige). Der Feed hier hat eine künstliche Verzögerung, damit der Ladezustand sichtbar wird.

```chart-demo backfill
```

## Einrichtung

Übergeben Sie dem Controller den Chart, die von ihm gesteuerte Serie und eine Datenquelle. Die Datenquelle löst ein Symbol auf und liefert Seiten von Bars zurück; der Controller fordert ältere Seiten automatisch an:

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { ChartDataController } from '@stocksharp/chart/data';

// A data source: resolve the symbol, then serve pages of bars ending before `to`.
const dataSource = {
  resolveSymbol(request) {
    return Promise.resolve({ id: request.symbol, priceFormat: { type: 'price', precision: 2, minMove: 0.01 } });
  },
  getBars(request) {
    // request: { symbol, resolution, to?, countBack }. Return { bars, hasMoreBefore, hasMoreAfter }.
    return fetchBars(request).then(bars => ({ bars, hasMoreBefore: bars.length > 0, hasMoreAfter: false }));
  },
  subscribeBars(request, listener) {
    // Push realtime bars via listener({ bar, isFinal }); return an unsubscribe function.
    return () => {};
  },
};

const chart = createChart(document.getElementById('chart'), { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const controller = new ChartDataController({
  chart,
  series,
  dataSource,
  initialCount: 300,               // bars loaded first
  historyCount: 250,               // bars per older page
  historyPrefetchThreshold: 40,    // prefetch when within 40 bars of the left edge
  autoPrefetch: true,              // load older bars automatically on scroll / zoom
});

// Optional: observe loading state and progress.
controller.subscribe(snap => {
  console.log(snap.loadedBars, snap.hasMoreBefore, snap.loadingHistory);
});

// Load the first page, then park the viewport near the right edge so there is room to scroll left.
controller.setSelection({ symbol: 'DEMO', resolution: '1h' }).then(() => {
  const loaded = controller.rawData().length;
  chart.timeScale().setVisibleLogicalRange({ from: Math.max(0, loaded - 90), to: loaded + 3 });
});
```

`historyPrefetchThreshold` gibt an, wie nah der sichtbare Bereich an den ältesten geladenen Bar heranreichen muss, bevor die nächste Seite abgerufen wird; `autoPrefetch` aktiviert dieses automatische Laden. Solange eine Seite unterwegs ist, ist `snapshot.loadingHistory` gleich `true`, sodass Sie einen Ladeindikator anzeigen können. Rufen Sie `controller.loadMoreBefore()` auf, um eine Seite manuell auszulösen, und `controller.dispose()`, um aufzuräumen.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Indikatoren](indicators.md)
