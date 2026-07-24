# 历史数据回填

图表无法一次性容纳整个合约的历史数据，因此它会加载最近若干根 K 线的窗口，并按需获取更早的数据。`ChartDataController` 会监视可见范围，当你向左边缘滚动或缩放时，它会从你的数据源拉取下一页历史数据并将其前置插入。

## 在线演示

向左边缘滚动或缩放——更早的 K 线会分页加载（注意观察状态提示文字）。这里的数据源带有人为设置的延迟，以便加载状态清晰可见。

```chart-demo backfill
```

## 设置

给控制器提供图表、它所驱动的序列以及一个数据源。数据源负责解析一个标的并返回若干页 K 线；控制器会自动请求更早的页：

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

`historyPrefetchThreshold` 表示视口必须离最早已加载的 K 线多近，才会触发获取下一页；`autoPrefetch` 用于开启这种自动加载。当某一页正在传输途中时，`snapshot.loadingHistory` 为 `true`，你可以据此显示加载动画。调用 `controller.loadMoreBefore()` 可手动触发加载一页，调用 `controller.dispose()` 可进行清理。

## 参见

- [JavaScript 图表](../javascript_charts.md)
- [蜡烛图](candlestick.md)
- [指标](indicators.md)
