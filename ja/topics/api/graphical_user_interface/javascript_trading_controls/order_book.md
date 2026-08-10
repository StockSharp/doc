# 板情報

`OrderBookWidget` は、買い気配と売り気配の価格帯、仲値、スプレッド、累積数量、市場センチメントを表示します。斜め表示と積み上げ表示、売買側の反転、5 または 10 段の深さ、`canvas` のデプスチャートを利用できます。

## 作成

```ts
import {
  OrderBookWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const book = OrderBookWidget.create(
  document.querySelector<HTMLElement>('#order-book')!,
  { followsActive: true },
  {
    host,
    onPriceSelected: (price, side) =>
      console.log('価格を入力', price, side),
    onPriceExecuted: (price, side) =>
      console.log('発注', price, side),
    maxDepth: () => 10,
    pixelRatio: () => window.devicePixelRatio || 1,
  },
);

book.setSymbol('BTC@IMEX');
```

価格帯を通常クリックすると `onPriceSelected` が呼び出され、Ctrl または Cmd を押しながらクリックすると `onPriceExecuted` が呼び出されます。数値の売買側 `0` は買い、`1` は売りを表します。売り気配をクリックすると買いが選択され、買い気配をクリックすると売りが選択されます。コントロールは取引の意図をホストへ渡しますが、注文自体は送信しません。

## 板情報のスナップショットと更新

最初のフレームは完全なスナップショットである必要があります。

```ts
book.applyFrame({
  symbol: 'BTC@IMEX',
  sequence: 1,
  isSnapshot: true,
  bids: [
    { price: 68_420.4, quantity: 2.5 },
    { price: 68_420.3, quantity: 4.1 },
  ],
  asks: [
    { price: 68_420.5, quantity: 1.8 },
    { price: 68_420.6, quantity: 3.2 },
  ],
});
```

以降の `isSnapshot: false` のフレームは差分として適用されます。数量 `0` は価格帯を削除します。`sequence` は欠番なく増加する必要があります。欠番があると、コントロールは新しいスナップショットを取得するために `host.trading.marketData.resubscribe(symbol, MarketDataLevels.Full)` を呼び出します。不正な価格帯や買い気配と売り気配が交差した板情報は、`host.log` へ渡されます。

## 表示と状態

```ts
book.setDepth(10);
book.setView('stacked');
book.setInvertSides(false);
book.setShowDepthChart(true);
```

`maxDepth()` を使用すると、ホストは小さな画面向けに価格帯の数を減らせます。`pixelRatio()` は `canvas` の内部描画バッファ密度を設定します。チャートの色は `host.presentation.canvasPalette()` が返します。

アクティブな銘柄に追従するパネルでは、表示設定が `host.preferences` に保存されます。固定されたインスタンスでは、パネルの状態に保存されます。作成時には `symbol`、`depth`、`view`、`invertSides`、`showDepthChart`、`followsActive` を渡せます。

`host.trading.marketData.getOrders()` の有効注文は、対応する価格帯の横に印が付きます。

## 公開メソッド

- `setSymbol`、`getSymbol` — 銘柄を管理します。
- `setDepth`、`getDepth` — 深さを設定して返します。
- `setView`、`setInvertSides`、`setShowDepthChart` — 表示を変更します。
- `getBids`、`getAsks` — 現在の価格帯を返します。
- `isFollowsActive` — パネルがアクティブな銘柄に追従するかを示します。
- `applyFrame` — スナップショットまたは差分を適用します。
- `dispose` — リソースを解放します。

## 関連項目

- [JavaScript トレーディングコントロール](../javascript_trading_controls.md)
- [ウォッチリスト](watchlist.md)
- [注文入力](order_entry.md)
