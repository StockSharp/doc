# Trade Feed

`TradeFeedWidget` displays public market trades and executions for the current portfolio. The market feed can be switched between a table and a bubble chart.

## Creating the control and streaming data

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

`setTrades` replaces the original set, while `addTrade` adds one streaming execution. The widget keeps no more than 50 table rows and the latest 500 prints for the chart.

## Bubble chart

On the chart, the horizontal axis represents time, the vertical axis represents price, bubble radius represents volume, and color represents the trade side. When adjacent prints are combined, the tooltip shows VWAP, total volume, and trade count. A trade is considered large when its volume is more than twice the moving average.

Canvas colors come from `host.presentation.canvasPalette()`. The selected mode is stored in `host.preferences` under a page-wide key.

## Additional instruments

In addition to the active symbol, the panel can pin extra ones:

```ts
await feed.addExtraSymbol('ETH@IMEX');
console.log(feed.getExtraSymbols());
await feed.removeExtraSymbol('ETH@IMEX');
```

Subscriptions at the `MarketDataLevels.Tape` level are created for them, while the bubble chart gets separate lanes with their own price scales. The list of additional instruments is stored in the state of a specific instance and can be supplied on creation as `{ extras: ['ETH@IMEX'] }`.

## Own trades

The second tab displays portfolio executions. Loading can be triggered explicitly:

```ts
await feed.loadMyTrades(host.trading.portfolioId(), 'BTC@IMEX');
```

The method calls `host.trading.api.getExecutions`. The rest of the market feed is always passed into the control from outside so that multiple panels can share one connection.

## Public methods

- `setActiveSymbol(symbol)` — set the primary instrument.
- `setTrades(...)`, `addTrade(...)` — replace or append to the market feed.
- `loadMyTrades(portfolioId, symbol)` — load own executions.
- `addExtraSymbol`, `removeExtraSymbol`, `getExtraSymbols` — manage pinned instruments.
- `dispose()` — remove subscriptions and release resources.

## See also

- [JavaScript Trading Controls](../trading_controls.md)
- [Trade history](trade_history.md)
- [Order book](order_book.md)
