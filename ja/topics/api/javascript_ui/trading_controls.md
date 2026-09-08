# JavaScript トレーディングコントロール

[StockSharp JavaScript トレーディングコントロール](https://github.com/StockSharp/JS-TradingControls) は、トレーディングターミナル向けのブラウザーパネル集です。パッケージは [`@stocksharp/trading-controls`](https://www.npmjs.com/package/@stocksharp/trading-controls) として npm に公開されており、すべてのコントロールを[オンラインデモ](https://stocksharp.github.io/JS-TradingControls/demo/)で確認できます。

![歩み値、板、銘柄一覧、注文入力、各種テーブルを備えた取引画面](../../../images/javascript_trading_controls.jpg)

画像には、別パッケージ `@stocksharp/chart` のローソク足チャートも表示されています。`@stocksharp/trading-controls` には、独立した 15 種類のコントロールが含まれます。

| コントロール | クラス | 識別子 |
|---|---|---|
| [有効注文](trading_controls/active_orders.md) | `ActiveOrdersWidget` | `activeOrders` |
| [ポジション](trading_controls/positions.md) | `PositionsWidget` | `positions` |
| [約定履歴](trading_controls/trade_history.md) | `TradeHistoryWidget` | `tradeHistory` |
| [ウォッチリスト](trading_controls/watchlist.md) | `WatchlistWidget` | `watchlist` |
| [注文入力](trading_controls/order_entry.md) | `OrderEntryWidget` | `orderEntry` |
| [板情報](trading_controls/order_book.md) | `OrderBookWidget` | `orderbook` |
| [歩み値](trading_controls/trade_feed.md) | `TradeFeedWidget` | `tradefeed` |
| [統計](trading_controls/statistics.md) | `StatisticsWidget` | `statistics` |
| [ログ](trading_controls/log_monitor.md) | `LogMonitorWidget` | `logMonitor` |
| [ストラテジー](trading_controls/strategies.md) | `StrategiesWidget` | `strategies` |
| [オプションデスク](trading_controls/option_desk.md) | `OptionDeskWidget` | `optionDesk` |
| [ボラティリティスマイル](trading_controls/option_smile.md) | `OptionSmileWidget` | `optionSmile` |
| [エクイティカーブ](trading_controls/equity.md) | `EquityWidget` | `equity` |
| [最適化ヒートマップ](trading_controls/optimization_heatmap.md) | `OptimizationHeatmapWidget` | `optimizationHeatmap` |
| [最適化サーフェス](trading_controls/optimization_surface.md) | `SurfaceWidget` | `optimizationSurface` |

識別子の値は、エクスポートされる `ControlTypes` オブジェクトから利用できます。なお、最適化サーフェスだけはクラス名と識別子が一致しません。クラス名は `SurfaceWidget`、識別子は `optimizationSurface` です。

## インストール

```bash
npm install @stocksharp/trading-controls
```

コントロールはテーブルを [@stocksharp/grids](grids.md) で描画します。これは通常の依存関係として自動的にインストールされます。一方、[@stocksharp/chart](charts.md) は **peer 依存関係**として宣言されています。npm はこれをインストールしないため、エクイティカーブやボラティリティスマイルを使う場合は自分でインストールしてください。これらはチャートエンジンの上に構築されています。

```bash
npm install @stocksharp/chart
```

パッケージはルートからのインポートに加えてサブパスも宣言しています。コントロールごとに 1 つずつ（`@stocksharp/trading-controls/watchlist` など）、補助モジュール（`/trading-host`、`/control-types`、`/formatters`、`/dom`、`/trading-data`）、そして TypeScript のソースを収めた並行の `/source/*` 系列です。最後のものは、コントロールを他のコードと一緒に自前のバンドラーでビルドする場合に使います。

基本スタイルは必須です。用意されているライト／ダークパレットを追加で読み込むか、独自の CSS 変数 `--t-*` で置き換えられます。

```ts
import '@stocksharp/trading-controls/styles.css';
import '@stocksharp/trading-controls/theme.css'; // 任意: 用意されているテーマ。
```

コントロールは [Bootstrap Icons](https://icons.getbootstrap.com/) のクラスを使用しますが、フォントや SVG 自体は同梱しません。ホストページでアイコンを別途読み込む必要があります。

バンドラーを使用しないページ向けには、グローバルオブジェクト `window.SSTradingControls` を作成する `dist/sstradingcontrols.js` が用意されています。

## 共通の作成方法

各コントロールは静的メソッド `create` で作成します。このメソッドはホストを検証し、独自の DOM を構築して、渡されたコンテナーへルート要素を追加します。

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('決済', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('ドテン', portfolioId, instrumentId, symbol),
    refreshPositions: () => console.log('更新'),
  },
);

positions.update([]);
```

第 2 引数は、保存されたインスタンスの状態です。第 3 引数の依存関係はコントロールごとに異なります。たとえば、ポジションパネルは決済とドテンのハンドラーを受け取り、板情報は価格の選択と発注のハンドラーを受け取ります。

## TradingHost コントラクト

コントロールは、グローバルな翻訳機能、設定ストア、取引接続、ウィンドウマネージャーへ直接アクセスしません。外部とのやり取りはすべて、1 つの `TradingHost` オブジェクトを通じて行われます。

| ホストのメンバー | 用途 |
|---|---|
| `isPrimary` | ページ上の主要なコントロールインスタンスを示します。 |
| `t(key, ...args)` | 表示テキストを翻訳し、引数を埋め込みます。 |
| `presentation` | 注文の売買区分、種類、状態、損益クラス、canvas パレットを整形します。 |
| `preferences`, `cache` | 永続設定と一時データを保存します。 |
| `trading.api` | 銘柄を検索し、約定を読み込みます。 |
| `trading.marketData` | 購読を管理し、有効注文を提供します。 |
| `trading.portfolioId()` | 現在のポートフォリオを返します。 |
| `trading.pickInstrument(...)` | 銘柄選択画面を開きます。 |
| `ticker` | 表示中の銘柄とその気配値を受け取ります。 |
| `allow(action)` | 操作が許可されているか確認します。 |
| `close`, `spawn`, `persistState`, `saveLayout` | パネルのライフサイクルと状態を管理します。 |
| `register`, `unregister`, `broadcast` | インスタンスを登録／解除し、その間で変更を配信します。 |
| `log(message)` | 診断メッセージを受け取ります。 |

すべてのメンバーが必須です。`assertHost` はコントロールを描画する前にネストされた関数まで検証し、欠落している正確なパスを報告します。アプリケーションで一部の機能を使用しない場合でも、必須コマンドには意味のある代替処理を渡せます。たとえば `log: console.warn` や、何もしない `saveLayout` です。

## ローカライズとスタイル

コントロールは、表示テキストを `host.t` からのみ取得します。235 個の最新キーを収めた完全な一覧が `@stocksharp/trading-controls/translation-keys.json` に同梱されています。これは配列ではなくオブジェクト `{ $comment, count, keys }` で、キー自体は `keys` フィールドに入っています。不明なキーはそのままユーザーに表示されるため、ホストは一覧のすべてに翻訳を定義する必要があります。

`styles.css` には規則が含まれますが、色、フォント、サイズには CSS 変数 `--t-*` を使用します。用意された `theme.css` を使わない場合は、アプリケーション側でこれらの変数を定義します。板情報とバブル表示の歩み値で使う canvas の色は、`host.presentation.canvasPalette()` が返します。

## リソースの解放

パネルを削除するときは `dispose()` を呼び出してください。このメソッドはハンドラーを解除し、コントロール固有のオブザーバーや購読があれば停止した後、`host.unregister` を呼び出します。

```ts
positions.dispose();
```

## ソースコードからビルドする

```bash
git clone https://github.com/StockSharp/JS-TradingControls.git
cd JS-TradingControls
npm install
npm test
npm run build
```

## 関連項目

- [JavaScript テーブル](grids.md)
- [JavaScript チャート](charts.md)
- [JS-TradingControls リポジトリ](https://github.com/StockSharp/JS-TradingControls)
- [オンラインデモ](https://stocksharp.github.io/JS-TradingControls/demo/)
