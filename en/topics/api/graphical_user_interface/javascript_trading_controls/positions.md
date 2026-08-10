# Positions

`PositionsWidget` displays open positions and a cash balance row pinned at the top. Positions are ordered alphabetically by default, while the balance does not participate in sorting, selection, or export.

## Creating and updating

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let positions!: PositionsWidget;

positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('close', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('reverse', portfolioId, instrumentId, symbol),
    refreshPositions: () => positions.update([]),
  },
);

positions.update([{
  portfolioId: 10,
  instrumentId: 42,
  instrument: 'BTC@IMEX',
  quantity: 0.25,
  avgPrice: 68_000,
  currentPrice: 68_420,
  unrealizedPnl: 105,
  realizedPnl: 20,
}]);

positions.updateBalance({
  available: 48_251,
  locked: 1_749,
  total: 50_000,
});
```

`update` replaces the position list. `applyDelta` updates one position by its portfolio and instrument combination; a row with zero quantity is removed. `updateBalance(null)` removes the pinned balance.

## Data and actions

Each row shows quantity, average price, current price, and a single total PnL calculated as the sum of `realizedPnl` and `unrealizedPnl`. Its color class is returned by `host.presentation.pnlClass`.

The row buttons invoke the host-provided `closePosition` and `reversePosition` handlers. The control does not create or send trading orders itself.

## Public methods

- `update(positions)` — replace all positions.
- `updateBalance(balance)` — set or remove the cash balance.
- `applyDelta(position)` — apply a streaming update to one position.
- `dispose()` — release resources.

The panel also supports refresh, sorting, a context menu, and exporting positions to XLSX.

## See also

- [JavaScript Trading Controls](../javascript_trading_controls.md)
- [Active orders](active_orders.md)
- [Order entry](order_entry.md)
