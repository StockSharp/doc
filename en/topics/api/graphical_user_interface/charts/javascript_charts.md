# JavaScript charts

[StockSharp JS Trading Charts](https://github.com/StockSharp/Charts) is a standalone browser chart library. It contains the dependency-free `sschart` canvas engine and the chart stack used by the StockSharp web terminal. A working version is available in the [live demo](https://stocksharp.github.io/Charts/demo/).

![StockSharp JavaScript trading chart](../../../../images/javascript_charts.jpg)

Unlike the Windows components from `StockSharp.Xaml.Charting`, this library runs in a browser and draws directly on an HTML `canvas`. The engine is exposed through the `SSChart` global and can also be imported from `src/sschart.ts` in a TypeScript build.

## Capabilities

- Candlestick, OHLC bar, line, area, histogram, Renko, Point and Figure, volume profile, cluster, and box series.
- Historical loading and real-time updates with `setData` and `update`.
- Trade markers, price lines, crosshair, zooming, scrolling, and automatic range calculation.
- An indicator engine with approximately 160 calculation implementations.
- Overlay indicators, synchronized oscillator panes, and a crosshair-driven legend.
- Light and dark themes, a context menu, an indicator dialog, and chart-type switching.

## Adding a chart to a page

The build produces `dist/sschart.js`, which publishes `window.SSChart`. Time values passed to the API are Unix timestamps in seconds.

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

Create a chart, add a series, and load the data:

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

Calling `update` with the current timestamp replaces the last point. A newer timestamp appends a point.

## Full terminal chart stack

The modules under `src/chart` extend the base engine with terminal features:

- `IndicatorEngine`, indicator renderers, settings, and the calculation catalog.
- A chart-type switcher for candles, bars, lines, areas, Heikin-Ashi, Renko, Point and Figure, clusters, and boxes.
- A legend, synchronized secondary panes, a context menu, and an indicator selection dialog.
- Recalculation of active indicators when real-time data changes.

Use `src/chart/app.ts` as the integration example for the complete stack.

## Building from source

Clone the repository and use the included npm scripts:

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

The development server serves the demo at `http://localhost:8791/demo/index.html`.

## See also

- [Charts repository](https://github.com/StockSharp/Charts)
- [Live demo](https://stocksharp.github.io/Charts/demo/)
- [Windows chart components](../charts.md)
