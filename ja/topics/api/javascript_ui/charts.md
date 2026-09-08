# JavaScript チャート

[StockSharp JS トレーディングチャート](https://github.com/StockSharp/JS-Charts) は、依存関係のないスタンドアロンなブラウザ用チャートライブラリです。[@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) として npm に公開されており、StockSharp Web ターミナルで使用されている `sschart` キャンバスエンジンを同梱しています。動作するバージョンは[ライブデモ](https://stocksharp.github.io/JS-Charts/demo/)で確認できます。

![StockSharp JavaScript トレーディングチャート](../../../images/javascript_charts.jpg)

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

- [ローソク足](charts/candlestick.md) — クラシックな OHLC ローソク足。
- [OHLC バー](charts/bar.md) — 縦のレンジバー上の始値/終値のティック。
- [ライン](charts/line.md) — 終値をつなぐ単一の折れ線。
- [エリア](charts/area.md) — グラデーション塗りつぶしを伴うライン。
- [ヒストグラム](charts/histogram.md) — 縦棒、通常はボリューム。
- [バンド](charts/band.md) — 上下のチャネル（エンベロープ、ボリンジャー）。
- [平均足 (Heikin-Ashi)](charts/heikin_ashi.md) — ノイズを除去する平滑化されたローソク足。
- [練行足 (Renko)](charts/renko.md) — 価格駆動のブロック、時間非依存。
- [ポイント・アンド・フィギュア](charts/point_figure.md) — 価格変動の X/O 列。
- [フットプリント](charts/footprint.md) — 各バー内のすべての価格における bid × ask のボリューム。
- [ボリュームプロファイル](charts/volume_profile.md) — POC とバリューエリアを伴う価格別ボリューム。
- [TPO（マーケットプロファイル）](charts/tpo.md) — セッションごとの各価格に費やされた時間。

系列タイプに加えて、チャートには約 160 種類の分析を備えた[インジケーターエンジン](charts/indicators.md)と、スクロールに応じて古いバーを読み込む[遅延ヒストリーバックフィル](charts/backfill.md)もあります。

同じ Web スタックによって描画されるビジュアルストラテジーエディターについては、[JavaScript ダイアグラム](diagram.md)を参照してください。

## エンジンの上に重なるレイヤー

ここまでで説明したのは基本エンジン、つまりエントリーポイント `@stocksharp/chart` です。それ以外の機能は、公開された個別のエントリーポイントに分かれています。必要なものをインポートするだけでよく、ソースをコピーする必要はありません。

| エントリーポイント | 提供する内容 |
|---|---|
| [チャートの外枠](charts/ui.md) — `@stocksharp/chart/ui` | すぐに使える外枠一式: 凡例、コンテキストメニュー、チャートタイプの切り替え、ペインマネージャー、インジケーターダイアログ。スタイルシート `@stocksharp/chart/ui.css` が必要です。 |
| `@stocksharp/chart/indicators` | `IndicatorEngine`、インジケーターのレンダラーと設定。計算そのものは [@stocksharp/indicators](charts/indicators.md) パッケージにあります。 |
| [チャートからの取引](charts/trading.md) — `@stocksharp/chart/trading` | トレーディングレイヤー: 注文ライン、現在の損益を伴うポジション、保護注文、気配値。ブローカーのことは知らず、意図を通知するだけで、実行はホストが行います。 |
| [描画ツール](charts/drawings.md) — `@stocksharp/chart/drawings` | 描画ツール、バーへのスナップ、独自図形の登録。 |
| [複数チャート](charts/workspace.md) — `@stocksharp/chart/workspace` | 同期された複数のチャート、銘柄の比較、レンジナビゲーター、インジケーターテンプレート。 |
| [レイアウトの保存](charts/persistence.md) — `@stocksharp/chart/persistence` | バージョン管理とマイグレーションを伴うレイアウトの保存と復元。 |
| [時間と取引セッション](charts/time.md) — `@stocksharp/chart/time` | 取引カレンダー、セッションとタイムゾーン、バー確定までのカウントダウン。 |
| `@stocksharp/chart/orderflow` | クラスター分析: フットプリントとボリュームプロファイル。 |

バンドラーを使わない場合も、同じ機能はブラウザ用パッケージで利用できます。`dist/sschartui.js` がグローバルオブジェクト `SSChartUI` を公開します。

## ソースからビルドする

リポジトリをクローンし、付属の npm スクリプトを使用します。

```bash
git clone https://github.com/StockSharp/JS-Charts.git
cd JS-Charts
npm install
npm run build
npm test
npm run serve
```

開発サーバーは `http://localhost:8791/demo/index.html` でデモを配信します。

## 関連項目

- [JavaScript ダイアグラム](diagram.md)
- [JS-Charts リポジトリ](https://github.com/StockSharp/JS-Charts)
- [ライブデモ](https://stocksharp.github.io/JS-Charts/demo/)
- [Windows 用チャートコンポーネント](../graphical_user_interface/charts.md)
