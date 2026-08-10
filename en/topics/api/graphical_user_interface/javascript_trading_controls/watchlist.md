# Watchlist

`WatchlistWidget` displays instruments and streaming quotes. The control supports search, favorites, categories, active instrument selection, and subscriptions only for symbols that are actually visible.

## Creating the control

```ts
import {
  WatchlistWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let watchlist!: WatchlistWidget;

watchlist = WatchlistWidget.create(
  document.querySelector<HTMLElement>('#watchlist')!,
  {},
  {
    host,
    onSelect: symbol => {
      watchlist.setCurrentSymbol(symbol);
      console.log('selected', symbol);
    },
  },
);
```

Creation automatically starts `init()`, which loads the list through `host.trading.api.searchInstruments('')`. The `symbol`, `name`, `exchange`, and `category` fields are used for each record.

Price updates are supplied from outside:

```ts
watchlist.onPriceUpdate('BTC@IMEX', 68_420.5);
```

The method updates only the required price and percentage cells, preserving the change animation. `setCurrentSymbol(symbol)` highlights the current instrument.

## Search, categories, and subscriptions

Search checks the symbol, name, and exchange. Tabs are created for all instruments, favorites, and the detected categories. Favorite symbols are stored in `host.preferences`.

The control subscribes the first 30 visible instruments at the `MarketDataLevels.Quotes` level. No more than 300 rows are rendered on screen, but filtering and export operate on the complete result set. Only an instance with `host.isPrimary === true` publishes visible quotes through `host.ticker`.

The percentage change is calculated from the first price received on the current UTC day. These base prices are cache data, so they are stored in `host.cache` rather than in user preferences.

## Public methods

- `init()` — load instruments and prepare subscriptions; invoked automatically by `create`.
- `setCurrentSymbol(symbol)` — mark the active instrument.
- `onPriceUpdate(symbol, price)` — apply a new price.
- `dispose()` — remove subscriptions and release resources.

## See also

- [JavaScript Trading Controls](../javascript_trading_controls.md)
- [Order book](order_book.md)
- [Order entry](order_entry.md)
