# 履歴のバックフィル

チャートは銘柄の全履歴を一度に保持できないため、直近のバーのウィンドウを読み込み、それより古いものは必要に応じて取得します。`ChartDataController` は表示範囲を監視し、左端に向かってスクロールまたはズームすると、データソースから履歴の次のページを取得して先頭に追加します。

## ライブデモ

左端に向かってスクロールまたはズームしてください。古いバーがページ単位で読み込まれます（ステータスキャプションに注目）。ここでのフィードには人為的な遅延が設定されており、読み込み中の状態が見えるようになっています。

```chart-demo backfill
```

## セットアップ

コントローラーに、チャート、それが駆動するシリーズ、そしてデータソースを渡します。データソースは銘柄を解決し、バーのページを返します。コントローラーは古いページを自動的に要求します。

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

`historyPrefetchThreshold` は、次のページが取得される前に、ビューポートが最も古い読み込み済みバーにどれだけ近づく必要があるかを示します。`autoPrefetch` はその自動読み込みを有効にします。ページの取得中は `snapshot.loadingHistory` が `true` になるため、スピナーを表示できます。ページを手動でトリガーするには `controller.loadMoreBefore()` を呼び出し、後始末には `controller.dispose()` を呼び出します。

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [ローソク足](candlestick.md)
- [インジケーター](indicators.md)
