# チャートの外枠

`createChartUi` は、エンジンの周りに完成した UI を組み立てます。ペインの見出し、十字カーソル連動の凡例、コンテキストメニュー、チャートタイプのメニュー、インジケーターダイアログです。エンジンは描画するだけで、その周囲のものはすべて独立したエントリーポイント `@stocksharp/chart/ui` にあります。スパークラインを 1 つ置くだけのページが、その分の重さを負担することはありません。

## 組み込み

このレイヤーはパッケージの別サブパスとして提供され、専用のスタイルシートを必要とします。

```ts
import { createChartUi, standaloneHost } from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';
```

バンドラーを使わない場合は、ブラウザー用パッケージ `dist/sschartui.js` を読み込みます。これはグローバルオブジェクト `SSChartUI` を公開します。読み込みは `dist/sschart.js` の**後**に行う必要があります。このレイヤーは、そのファイルが公開したグローバルオブジェクトからエンジンを読み取るのであって、エンジンの 2 つ目のコピーを持っているわけではないからです。

## 作成と更新

このレイヤーには、チャートが作成された要素、ページのホスト、コンテキストメニュー用の価格ソース、凡例のメニュー用のチャートタイプ一覧が必要です。

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  ChartType,
  createChartUi,
  localChartUiStorage,
  standaloneHost,
  type ChartUi,
} from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';

declare const candles: { time: number; open: number; high: number; low: number; close: number; volume?: number }[];

const container = document.querySelector<HTMLElement>('#chart')!;

const chart = createChart(container, { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
series.setData(candles);

const ui: ChartUi = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [
    { value: ChartType.Candle, label: 'Candles', icon: 'bi bi-bar-chart-fill' },
    { value: ChartType.Line, label: 'Line', icon: 'bi bi-graph-up' },
  ],
  storage: localChartUiStorage('sschart'),
});

ui.setCandles(candles);
```

`setCandles` は、ローソク足のウィンドウが変わるたびに呼び直します。銘柄の変更、チャートタイプの切り替え、履歴ページの到着などです。1 回の呼び出しで、インジケーターエンジンと凡例の両方が更新されます。

`createChartUi` のオプション:

| オプション | 用途 |
|---|---|
| `container` | チャートが作成された要素。その周囲にパネルが構築されます。 |
| `host` | 翻訳、書式設定、メッセージ。 |
| `priceSource` | コンテキストメニュー用のピクセル → 価格の変換。 |
| `chartTypes` | チャートタイプメニューの項目を表示順に並べたもの。空のリストならメニューは描画されません。 |
| `storage` | お気に入りのインジケーターとテンプレートの保存先。既定ではメモリ内です。 |
| `dialogRoot` | インジケーターダイアログの独自マークアップ。指定しない場合、マークアップが構築されて `body` に追加されます。 |
| `modal` | ダイアログの開閉に関する独自の実装。 |
| `provideItems` | コンテキストメニューに追加するページ側の項目。レイヤー自身の項目より上に置かれます。 |

## createChartUi が返すもの

戻り値は、すでに相互に接続された同じオブジェクト群です。どれにも直接アクセスできます。

| フィールド | 内容 |
|---|---|
| `engine` | `IndicatorEngine` — インジケーターの計算とライフサイクル。 |
| `renderer` | `IndicatorRenderer` — インジケーターの出力を描くシリーズ。 |
| `paneManager` | `ChartPaneManager` — ペインの見出し、そのメニュー、復元。 |
| `legend` | `ChartLegend` — OHLCV の行と、十字カーソル位置のインジケーター値。 |
| `dialog` | `IndicatorDialog` — カタログ、検索、パラメーター、有効なインジケーター。 |
| `menu` | `ChartContextMenu` — 右クリックのメニュー。 |
| `indicators` | `IndicatorController` — 追加済みインジケーターの、取り消し可能な編集。 |
| `templates` | `IndicatorTemplateController` — 持ち運べるインジケーター設定のテンプレート。 |

## 公開メソッド

- `setCandles(candles)` — 現在のローソク足ウィンドウをインジケーターエンジンと凡例に渡します。
- `showIndicators()` — インジケーターダイアログを開きます。
- `dispose()` — メニュー、ダイアログ、凡例、ペインを取り外します。レイヤーが作成したダイアログのマークアップは削除されます。

## ページのホスト

このレイヤーのどのモジュールもグローバルオブジェクトを参照しません。文言、数値、メッセージは `ChartUiHost`、すなわち `translate`、`formatters`、`notify` のフィールドを持つオブジェクトから届きます。

```ts
import { consoleNotify, createTranslate, defaultChartFormatters, type ChartUiHost } from '@stocksharp/chart/ui';

const host: ChartUiHost = {
  translate: createTranslate({ 'Indicators': 'インジケーター', 'Add indicator…': 'インジケーターを追加…' }),
  formatters: { ...defaultChartFormatters, price: value => value.toFixed(2) },
  notify: consoleNotify,
};
```

辞書はフラットで、英語の原文をキーにします。応答のないキーはそれ自身、つまり読める英語の文字列を返すのであって、翻訳欠落を示すマーカーにはなりません。差し込みは位置指定で、`{0}`、`{1}` を使います。

- `standaloneHost` — すべてを自前でまかなうホスト。英語のテキスト、数値の大きさに応じた書式、コンソールへのメッセージ。
- `identityTranslate` — 単一言語のページ向けの翻訳。プレースホルダーの差し込みは維持されます。
- `defaultChartFormatters` — `price`、`volume`、`time`（Unix 秒、書式は `YYYY-MM-DD HH:MM`）。
- `consoleNotify` — ブラウザーのコンソールへのメッセージ出力。レベルは `success`、`info`、`warning`、`error`。
- `createPlainModalController(root)` — モーダルウィンドウのライブラリを持たないページ向けのダイアログ開閉。背景の覆いと `Escape` での閉じる操作を備えます。ウィンドウの外側をクリックしてもダイアログは閉じません。

## ストレージ

お気に入りのインジケーターとテンプレートは `ChartUiStorage`、すなわち `load(key)` と `save(key, value)` の 2 つの関数を通じて保存されます。

- `inMemoryChartUiStorage` — 既定値。データはページと同じ寿命です。
- `localChartUiStorage(prefix)` — プレフィックス付きの `localStorage` ラッパー。ページ上の 2 つのチャートが互いのお気に入りを上書きしないようにします。

## チャートタイプ

`ChartTypeSwitcher` は、同じバーのウィンドウをローソク足、バー、ライン、エリア、Heikin-Ashi、Renko、Point & Figure で描き直します。タイプの変更は別のレンダラーを意味するため、シリーズは作り直され、それまでのインスタンスは無効になります。

```ts
import {
  ChartType,
  ChartTypeSwitcher,
  allChartTypes,
  defaultChartTypePalette,
  parseChartType,
} from '@stocksharp/chart/ui';

const switcher = new ChartTypeSwitcher({
  chart,
  series,
  initialType: ChartType.Candle,
  availableTypes: allChartTypes,
  palette: defaultChartTypePalette,
  host: standaloneHost,
});
switcher.setRawCandles(candles);

switcher.onSeriesChanged(next => ui.menu.setPriceSource(next));

ui.legend.onChartTypeChange = value => {
  const type = parseChartType(value);
  if (type === null) return;

  switcher.switchType(type);
  ui.setCandles(switcher.getIndicatorCandles());
};
```

`parseChartType` は、ページが自分で保持していた文字列（保存されたレイアウトやボタンの属性）からタイプを読み取り、該当するタイプがなければ `null` を返します。`getIndicatorCandles` は、切り替え後にインジケーターの計算対象となるバーを返します。Renko と Point & Figure は元のバーを独自のものに組み替えます。それが起きたかどうかは `isDerivedChartType(type)` が答えます。そうしたバーには出来高もありません。その他のメソッドは `getCurrentSeries`、`getCurrentType`、`getAvailableTypes`、`updatePrice` です。

## コンテキストメニュー

`ChartContextMenu` は自分の項目を追加しません。項目はページが `provideItems` で、項目のグループを返すことで与えます。グループの間には区切り線が描かれ、空のグループは何も占めません。項目は、キー `key`、テキスト `label`、任意の `icon`、`tone`、`disabled`、そしてメソッド `invoke` で表されます。トーンは `ChartContextMenuTone` の値 `Neutral`、`Positive`、`Negative` で指定します。

```ts
import { ChartContextMenuTone, createChartUi, standaloneHost } from '@stocksharp/chart/ui';

const ui = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [],
  provideItems: context => [[
    {
      key: 'buy',
      label: `${context.priceText} で買う`,
      icon: 'bi bi-arrow-up-circle',
      tone: ChartContextMenuTone.Positive,
      invoke: () => placeOrder('buy', context.price),
    },
  ]],
});
```

これらの項目に対して、`createChartUi` は自身のグループ、すなわち `Add indicator…` と `Add pane…` を追加します。どちらも `host.translate` を経由します。`ChartContextMenuMode` は、価格チャート上のメニュー（`Chart`、カーソル下に価格がある）とサブペインの見出し上のメニュー（`Pane`、価格がない）を区別します。メソッドは `init`、`setPriceSource`、`openAt`、`close`、`dispose` です。

## その他のエクスポート

- `ChartLegend` と `fullscreenMenuLayer` — 凡例と、そのフローティングメニューが開かれるレイヤー（全画面要素があればそれ、なければ `body`）。凡例のメソッドは `init`、`setRawCandles`、`setChartType`、`setIndicatorEngine`、`refresh`、`dispose`、コールバックは `onEditIndicator` と `onChartTypeChange` です。
- `ChartPaneManager` — エンジン組み込みのペインに対する外枠。`init`、`addPane`、`removePane`、`restorePane`、`getChart`、`getPanes`、`getPaneByMeasure`、`setPaneTitle`、`getValuesElement`、`legendLayer`、`resize`、`dispose`。
- `IndicatorDialog` と `createIndicatorCatalogController` — インジケーターダイアログと、そのカタログのモデル。ダイアログのメソッドは `show`、`showForPane`、`showEdit`、`hide`、`dispose` です。
- `IndicatorEngine`、`IndicatorRenderer`、`IndicatorSettings` — ダイアログが制御するインジケーターの仕組み。ここからも `@stocksharp/chart/indicators` からも公開されています。
- 型 `LegendBar`、`LegendChartType`、`LegendChart`、`LegendPaneHost`、`LegendIndicatorEngine`、`IndicatorPaneChart`、`IndicatorPaneHost`、`ChartContextMenuProvider`、`PriceCoordinateSource`、`ChartTypePalette`、`ModalController` — 構造的なコントラクトです。ペインの配置やインジケーターの計算を自分で行うページは、これらを自前のオブジェクトで実装します。

## 関連項目

- [JavaScript チャート](../charts.md)
- [インジケーター](indicators.md)
- [履歴のバックフィル](backfill.md)
- [ローソク足](candlestick.md)
