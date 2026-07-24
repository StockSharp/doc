# JavaScript チャート

[StockSharp JS トレーディングチャート](https://github.com/StockSharp/Charts) は、依存関係のないスタンドアロンなブラウザ用チャートライブラリです。[@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) として npm に公開されており、StockSharp Web ターミナルで使用されている `sschart` キャンバスエンジンを同梱しています。動作するバージョンは[ライブデモ](https://stocksharp.github.io/Charts/demo/)で確認できます。

![StockSharp JavaScript トレーディングチャート](../../../../images/javascript_charts.jpg)

`StockSharp.Xaml.Charting` の Windows コンポーネントとは異なり、このライブラリはブラウザ上で動作し、HTML の `canvas` に直接描画します。エンジンは `SSChart` グローバル（`dist/sschart.js` から）を通じて公開され、npm パッケージから ECMAScript モジュールとしてインポートすることもできます（`import { createChart, CandlestickSeries } from '@stocksharp/chart'`）。

## ライブデモ

以下のチャートは、このページ上で実際に動作しているエンジンです。ボリュームヒストグラムと移動平均線を伴ったローソク足を表示しています。ドラッグでスクロール、ホイールでズーム、（右上の）展開ボタンを押すと全画面で開きます。

```chart-demo overview
```

## 機能

- 価格系列のフルセット: ローソク足、OHLC バー、ライン、エリア、ヒストグラム、バンド、さらにこれらから派生した Heikin-Ashi、Renko、Point & Figure タイプ。
- 精密なオーダーフロー分析: フットプリント、ボリュームプロファイル、TPO（マーケットプロファイル）。
- `setData` と `update` による履歴読み込みとリアルタイム更新。
- 約定マーカー、価格ライン、十字カーソル、ズーム、スクロール、自動レンジ計算。
- 約 160 種類の計算実装を備えたインジケーターエンジン。
- オーバーレイインジケーター、同期されたオシレーターペイン、十字カーソル連動の凡例。
- ライトとダークのテーマ、コンテキストメニュー、インジケーターダイアログ、チャートタイプの切り替え。

## インストール

npm からパッケージをインストールし、ES モジュールをインポートします。

```bash
npm install @stocksharp/chart
```

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
```

または、バンドラーを使わずに、以下に示すように `<script>` タグでビルド済みの `SSChart` グローバルを読み込むこともできます。

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
  upColor: '#26a69a',
  downColor: '#ef5350',
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

## チャートモード

各系列タイプには、ライブデモとそれを構成する JavaScript を含む独自のトピックがあります。

- [ローソク足](javascript_charts/candlestick.md) — クラシックな OHLC ローソク足。
- [OHLC バー](javascript_charts/bar.md) — 縦のレンジバー上の始値/終値のティック。
- [ライン](javascript_charts/line.md) — 終値をつなぐ単一の折れ線。
- [エリア](javascript_charts/area.md) — グラデーション塗りつぶしを伴うライン。
- [ヒストグラム](javascript_charts/histogram.md) — 縦棒、通常はボリューム。
- [バンド](javascript_charts/band.md) — 上下のチャネル（エンベロープ、ボリンジャー）。
- [平均足 (Heikin-Ashi)](javascript_charts/heikin_ashi.md) — ノイズを除去する平滑化されたローソク足。
- [練行足 (Renko)](javascript_charts/renko.md) — 価格駆動のブロック、時間非依存。
- [ポイント・アンド・フィギュア](javascript_charts/point_figure.md) — 価格変動の X/O 列。
- [フットプリント](javascript_charts/footprint.md) — 各バー内のすべての価格における bid × ask のボリューム。
- [ボリュームプロファイル](javascript_charts/volume_profile.md) — POC とバリューエリアを伴う価格別ボリューム。
- [TPO（マーケットプロファイル）](javascript_charts/tpo.md) — セッションごとの各価格に費やされた時間。

系列タイプに加えて、チャートには約 160 種類の分析を備えた[インジケーターエンジン](javascript_charts/indicators.md)と、スクロールに応じて古いバーを読み込む[遅延ヒストリーバックフィル](javascript_charts/backfill.md)もあります。

同じ Web スタックによって描画されるビジュアル戦略エディターについては、[JavaScript ダイアグラム](../javascript_diagram.md)を参照してください。

## ターミナルの完全なチャートモジュール群

`src/chart` 配下のモジュールは、基本エンジンにターミナル機能を追加します。

- `IndicatorEngine`、インジケーターレンダラー、設定、計算カタログ。
- ローソク足、バー、ライン、エリア、Heikin-Ashi、Renko、Point & Figure のチャートタイプ切り替え。
- 凡例、同期された補助ペイン、コンテキストメニュー、インジケーター選択ダイアログ。
- リアルタイムデータ変更時の有効なインジケーターの再計算。

完全なスタックの統合例として `src/chart/app.ts` を使用してください。

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

- [JavaScript ダイアグラム](../javascript_diagram.md)
- [Charts リポジトリ](https://github.com/StockSharp/Charts)
- [ライブデモ](https://stocksharp.github.io/Charts/demo/)
- [Windows 用チャートコンポーネント](../charts.md)
