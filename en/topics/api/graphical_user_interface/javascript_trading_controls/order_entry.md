# Order Entry

`OrderEntryWidget` is a two-sided buy and sell panel. It supports market, limit, stop, and stop-limit orders, displays only the fields relevant to the selected type, and validates values before passing them to the host.

## Creating and configuring an instrument

```ts
import {
  OrderEntrySides,
  OrderEntryTypes,
  OrderEntryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const pad = OrderEntryWidget.create(
  document.querySelector<HTMLElement>('#order-entry')!,
  {},
  {
    host,
    submitOrder: (side, values) =>
      console.log('submit', side, values),
  },
);

pad.setInstrument({
  symbol: 'BTC@IMEX',
  lotSize: 0.001,
  tickSize: 0.1,
  minVolume: 0.001,
  maxVolume: 5,
});

pad.setOrderType(OrderEntryTypes.Limit);
pad.setBbo(68_420.4, 68_420.5);
pad.setAvailable(OrderEntrySides.Buy, 48_251);
pad.setMaxQuantity(OrderEntrySides.Buy, 0.7);
pad.setQuantity(0.01);
```

`OrderEntryTypes` contains `Market`, `Limit`, `Stop`, and `StopLimit`, while `OrderEntrySides` contains `Buy` and `Sell`.

## Validation and submission

The control validates positive values, lot size, price step, and minimum and maximum volume. Optional Take Profit and Stop Loss fields are enabled with `toggleTpSl`.

`submit(side)` first invokes `validate(side)`. When the data is valid, the `submitOrder` handler receives the side and an object:

```ts
interface OrderEntryValues {
  type: 'market' | 'limit' | 'stop' | 'stoplimit';
  quantity: number;
  limitPrice: number | null;
  stopPrice: number | null;
  takeProfit: number | null;
  stopLoss: number | null;
}
```

The control itself does not select a portfolio, check the connection, or send the order to the server. These actions remain in the host handler.

## Prices, quantity, and state

- `setBbo(bid, ask)` updates the best bid and ask.
- `applyBbo(side)` inserts the opposite BBO side into the selected column for immediate execution.
- `setAvailable(side, value)` sets the displayed available balance.
- `setMaxQuantity(side, value)` sets the maximum from which the percentage buttons calculate quantity.
- `applyPercent(side, pct)` applies the requested percentage of the maximum set through `setMaxQuantity`.
- `preselect(side)` highlights the side after a “trade on click” gesture.
- `setEnabled(false)` prevents submission without discarding entered values.
- `getValues(side)` and `validate(side)` let the form be inspected externally.

The static `toApiType` method converts `Limit` to `0`, `Market` to `1`, and `Stop` and `StopLimit` to `2`.

## See also

- [JavaScript Trading Controls](../javascript_trading_controls.md)
- [Order book](order_book.md)
- [Positions](positions.md)
