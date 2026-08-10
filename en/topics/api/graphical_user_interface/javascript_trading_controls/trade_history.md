# Trade History

`TradeHistoryWidget` is a table of executions for the current portfolio. The latest trades appear at the top; each row shows the time, instrument, side, quantity, price, trade identifier, and order identifier.

## Creating and loading

```ts
import {
  TradeHistoryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const history = TradeHistoryWidget.create(
  document.querySelector<HTMLElement>('#history')!,
  {},
  { host },
);

await history.refresh();
```

Creating the control does not start loading automatically. The `refresh()` method obtains the current portfolio through `host.trading.portfolioId()` and, when the host permits loading trade history, invokes:

```ts
const portfolioId = host.trading.portfolioId();
if (portfolioId && host.allow('load trade history'))
  host.trading.api.getExecutions(portfolioId, null, 200);
```

This approach is important when switching portfolios: the identifier is read immediately before each refresh rather than stored when the panel is created.

## Behavior

The panel is read-only. Users can sort and select rows, open the context menu, refresh the data, and export visible columns to XLSX. Loading errors are passed to `host.log`.

The control's public API consists of `refresh(): Promise<void>` and `dispose(): void`.

## See also

- [JavaScript Trading Controls](../javascript_trading_controls.md)
- [Active orders](active_orders.md)
- [Trade feed](trade_feed.md)
