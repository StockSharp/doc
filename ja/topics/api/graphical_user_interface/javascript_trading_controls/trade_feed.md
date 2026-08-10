# 歩み値

`TradeFeedWidget` は、市場全体の約定と現在のポートフォリオの約定を表示します。市場の歩み値は、テーブル表示とバブルチャート表示を切り替えられます。

## 作成とデータストリーム

```ts
import {
  TradeFeedWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const feed = TradeFeedWidget.create(
  document.querySelector<HTMLElement>('#trade-feed')!,
  {},
  { host },
);

feed.setActiveSymbol('BTC@IMEX');

feed.setTrades([{
  symbol: 'BTC@IMEX',
  side: 0,
  price: 68_420.5,
  quantity: 0.25,
  time: new Date().toISOString(),
}]);

feed.addTrade({
  symbol: 'BTC@IMEX',
  side: 1,
  price: 68_419.9,
  quantity: 0.4,
  time: new Date().toISOString(),
});
```

`setTrades` は初期データセットを置き換え、`addTrade` はストリームから 1 件の約定を追加します。ウィジェットが保持するのは、テーブルでは最大 50 行、チャートでは直近 500 件の約定点です。

## バブルチャート

チャートの横軸は時刻、縦軸は価格、バブルの半径は数量、色は売買側を表します。隣接する約定点が統合されると、ツールチップには VWAP、合計数量、約定件数が表示されます。数量が移動平均の 2 倍を超える約定は、大口約定と見なされます。

`canvas` の色は `host.presentation.canvasPalette()` から取得します。選択した表示モードは、ページ共通のキーで `host.preferences` に保存されます。

## 追加銘柄

パネルは、アクティブなシンボルに加えて別の銘柄を固定できます。

```ts
await feed.addExtraSymbol('ETH@IMEX');
console.log(feed.getExtraSymbols());
await feed.removeExtraSymbol('ETH@IMEX');
```

追加銘柄には `MarketDataLevels.Tape` レベルの購読が作成され、バブルチャートでは個別の価格スケールを持つ独立した帯が使用されます。追加銘柄の一覧は各インスタンスの状態に保存され、作成時に `{ extras: ['ETH@IMEX'] }` として渡すこともできます。

## 自分の約定

2 番目のタブにはポートフォリオの約定が表示されます。読み込みは明示的に呼び出せます。

```ts
await feed.loadMyTrades(host.trading.portfolioId(), 'BTC@IMEX');
```

このメソッドは `host.trading.api.getExecutions` を呼び出します。複数のパネルが 1 つの接続を利用できるよう、それ以外の市場の歩み値は常に外部からコントロールへ渡します。

## 公開メソッド

- `setActiveSymbol(symbol)` — 主要な銘柄を設定します。
- `setTrades(...)`、`addTrade(...)` — 市場の歩み値を置き換えるか追加します。
- `loadMyTrades(portfolioId, symbol)` — 自分の約定を読み込みます。
- `addExtraSymbol`、`removeExtraSymbol`、`getExtraSymbols` — 固定銘柄を管理します。
- `dispose()` — 購読を削除し、リソースを解放します。

## 関連項目

- [JavaScript トレーディングコントロール](../javascript_trading_controls.md)
- [約定履歴](trade_history.md)
- [板情報](order_book.md)
