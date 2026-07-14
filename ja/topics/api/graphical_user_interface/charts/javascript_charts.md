# JavaScript チャート

[StockSharp の JavaScript 取引チャート](https://github.com/StockSharp/Charts)は、ブラウザー向けの独立したチャートライブラリです。実行時依存関係のない canvas エンジン `sschart` と、StockSharp Web ターミナルで使用されるチャートモジュール群が含まれています。動作するバージョンは[ライブデモ](https://stocksharp.github.io/Charts/demo/)で確認できます。

![StockSharp の JavaScript 取引チャート](../../../../images/javascript_charts.jpg)

`StockSharp.Xaml.Charting` の Windows コンポーネントとは異なり、このライブラリはブラウザーで動作し、HTML の `canvas` に直接描画します。エンジンはグローバルオブジェクト `SSChart` として公開され、TypeScript ビルドでは `src/sschart.ts` からインポートすることもできます。

## 機能

- ローソク足、OHLC バー、ライン、エリア、ヒストグラム、Renko、ポイント・アンド・フィギュア、ボリュームプロファイル、クラスター、ボックスの各系列。
- `setData` と `update` による履歴読み込みとリアルタイム更新。
- 約定マーカー、価格ライン、十字カーソル、ズーム、スクロール、自動レンジ計算。
- 約 160 種類の計算実装を備えたインジケーターエンジン。
- オーバーレイインジケーター、同期されたオシレーターペイン、十字カーソル連動の凡例。
- ライトとダークのテーマ、コンテキストメニュー、インジケーターダイアログ、チャートタイプの切り替え。

## ページにチャートを追加する

ビルドによって `dist/sschart.js` が生成され、`window.SSChart` が公開されます。API に渡す時刻値は秒単位の Unix タイムスタンプです。

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

チャートを作成し、系列を追加してデータを読み込みます。

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

現在のタイムスタンプで `update` を呼び出すと、最後のポイントが置き換えられます。より新しいタイムスタンプではポイントが追加されます。

## ターミナルの完全なチャートモジュール群

`src/chart` 配下のモジュールは、基本エンジンにターミナル機能を追加します。

- `IndicatorEngine`、インジケーターレンダラー、設定、計算カタログ。
- ローソク足、バー、ライン、エリア、Heikin-Ashi、Renko、ポイント・アンド・フィギュア、クラスター、ボックスのタイプ切り替え。
- 凡例、同期された補助ペイン、コンテキストメニュー、インジケーター選択ダイアログ。
- リアルタイムデータ変更時の有効なインジケーターの再計算。

完全なモジュール群の統合例として `src/chart/app.ts` を使用してください。

## ソースからビルドする

リポジトリをクローンし、付属の npm スクリプトを使用します。

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

開発サーバーは `http://localhost:8791/demo/index.html` でデモを配信します。

## 関連項目

- [Charts リポジトリ](https://github.com/StockSharp/Charts)
- [ライブデモ](https://stocksharp.github.io/Charts/demo/)
- [Windows 用チャートコンポーネント](../charts.md)
