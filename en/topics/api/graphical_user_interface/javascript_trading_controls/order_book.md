# Order Book

`OrderBookWidget` displays bid and ask levels, the mid-price, spread, cumulative volume, and market sentiment. Diagonal and stacked views, side inversion, a depth of 5 or 10 levels, and a canvas depth chart are available.

## Creating the control

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
      console.log('prefill', price, side),
    onPriceExecuted: (price, side) =>
      console.log('execute', price, side),
    maxDepth: () => 10,
    pixelRatio: () => window.devicePixelRatio || 1,
  },
);

book.setSymbol('BTC@IMEX');
```

A regular click on a level invokes `onPriceSelected`, while a Ctrl- or Cmd-click invokes `onPriceExecuted`. Numeric side `0` means buy and `1` means sell: clicking an ask selects a buy, while clicking a bid selects a sell. The control passes the intent to the host but does not send an order itself.

## Order book snapshots and updates

The first frame must be a complete snapshot:

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

Subsequent frames with `isSnapshot: false` are applied as updates. A quantity of `0` removes a level. `sequence` must increase without gaps; when a gap occurs, the control invokes `host.trading.marketData.resubscribe(symbol, MarketDataLevels.Full)` to obtain a new snapshot. Invalid levels and a crossed order book are passed to `host.log`.

## View and state

```ts
book.setDepth(10);
book.setView('stacked');
book.setInvertSides(false);
book.setShowDepthChart(true);
```

`maxDepth()` lets the host reduce the number of levels for a small screen, while `pixelRatio()` sets the canvas backing-store density. Chart colors are returned by `host.presentation.canvasPalette()`.

For a panel that follows the active instrument, view settings are stored in `host.preferences`. A pinned instance keeps them in panel state. On creation, `symbol`, `depth`, `view`, `invertSides`, `showDepthChart`, and `followsActive` can be supplied.

Active orders from `host.trading.marketData.getOrders()` are marked next to their corresponding levels.

## Public methods

- `setSymbol`, `getSymbol` — manage the instrument.
- `setDepth`, `getDepth` — set and return the depth.
- `setView`, `setInvertSides`, `setShowDepthChart` — change the view.
- `getBids`, `getAsks` — return the current levels.
- `isFollowsActive` — report whether the panel follows the active instrument.
- `applyFrame` — apply a snapshot or update.
- `dispose` — release resources.

## See also

- [JavaScript Trading Controls](../javascript_trading_controls.md)
- [Watchlist](watchlist.md)
- [Order entry](order_entry.md)
