# History backfill

A chart cannot hold a whole instrument's history at once, so it loads a window of recent bars and fetches older ones on demand. The `ChartDataController` watches the visible range and, as you scroll or zoom toward the left edge, pulls the next page of history from your data source and prepends it.

## Live demo

Scroll or zoom toward the left edge — older bars load in pages (watch the status caption). The feed here has an artificial delay so the loading state is visible.

```chart-demo backfill
```

## Setup

Give the controller the chart, the series it drives, and a data source. The data source resolves a symbol and returns pages of bars; the controller requests older pages automatically:

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

`historyPrefetchThreshold` is how close to the oldest loaded bar the viewport must get before the next page is fetched; `autoPrefetch` turns that automatic loading on. While a page is in flight `snapshot.loadingHistory` is `true`, so you can show a spinner. Call `controller.loadMoreBefore()` to trigger a page manually, and `controller.dispose()` to clean up.

## See also

- [JavaScript charts](../charts.md)
- [Candlestick](candlestick.md)
- [Indicators](indicators.md)
