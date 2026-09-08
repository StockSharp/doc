# Option desk

`OptionDeskWidget` shows one series of an option chain: calls on the left, puts mirrored on the right, with the strike and the intrinsic value between them. Volume, open interest, and volatilities are rendered not only as a number but also as a bar, so the chain reads by shape and not by values alone.

![Option desk with call and put sides around the strikes](../../../../images/javascript_controls_option_desk.png)

## Creating and updating

```ts
import {
  OptionDeskWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const desk = OptionDeskWidget.create(
  document.querySelector<HTMLElement>('#option-desk')!,
  {},
  { host },
);

desk.update(
  [{
    strike: 68_000,
    call: {
      symbol: 'BTC-68000-C',
      bid: 1_240,
      ask: 1_265,
      last: 1_250,
      theoretical: 1_248,
      volume: 320,
      openInterest: 1_480,
      ivBid: 0.42,
      ivAsk: 0.44,
      ivLast: 0.43,
      historicalVolatility: 0.39,
    },
    put: {
      symbol: 'BTC-68000-P',
      bid: 820,
      ask: 845,
      volume: 210,
      openInterest: 960,
      ivLast: 0.47,
    },
  }],
  {
    assetPrice: 68_420,
    timeToExpiry: 0.08,
    riskFree: 0.05,
    dividend: 0,
  },
);
```

The only dependency is `host`; the control has no optional ones. The second argument of `create` is the panel state, which the control does not use.

`update(strikes, context)` replaces the whole chain. Both arguments are passed together: the chain and the underlying price are one observation, and updating them separately would show greeks computed against a price that has already moved. `context` is optional and empty by default.

## Chain context

`OptionChainContext` describes what the series is valued against: `assetPrice` is the price of the underlying, `timeToExpiry` is the time to expiry in years, and `riskFree` and `dividend` are rates as fractions (`0.05` means five percent). Without `assetPrice` the desk still shows the quotes, but the intrinsic value is zero and the rows are not split into in-the-money and out-of-the-money. Without `assetPrice` or `timeToExpiry` the greeks are not computed, and the cell stays empty rather than zero.

## Greeks

Greeks arrive in one of two ways. If the host computes them itself, it passes a ready `greeks` object on the strike side, and the desk shows what it received. If the host sends a volatility, the greeks are computed on the spot with Black-Scholes from the first available value in the order `ivLast`, `ivBid`, `ivAsk`, `historicalVolatility`. Neither way is a fallback for the other — they are two shapes of the host.

The number of decimals is chosen from the data: four significant digits for the smallest value in the column, but no fewer than two and no more than eight decimals. The count is one per column and shared by both sides, so delta does not turn into `0.0000`, and gamma and its mirror column are written the same way.

## Columns and presentation

The column order runs outwards from the strike: volatilities and quotes closer to the middle, greeks at the edges; the put side is the same in reverse order. Sorting is by strike, ascending: the chain reads like a ladder.

The `callRho`, `callTheta`, `callHv`, `callTheor` columns and their mirrors `putRho`, `putTheta`, `putHv`, `putTheor` are hidden by default — the table context menu brings them back.

The bars are scaled differently. Volume and open interest are scaled per side, because calls and puts trade in different sizes. Volatilities use a single scale across both sides, otherwise the skew between the sides would disappear. The bar is drawn with the element's width, without a canvas.

A row gets the `option-itm-call` class for strikes below the asset price and `option-itm-put` for the rest; with `assetPrice` missing it gets only `option-row`. Volatilities are shown as percentages with two decimals, prices in the package's common price format.

The desk stores no settings: it never touches `host.preferences` or `host.cache`, and the column set and the sorting live in the current instance.

## What the host does

All visible text comes from `host.t` — the panel title, the column captions, the empty-table caption, the context menu items. The close button calls `host.close()`: the panel does not remove itself. On creation the control calls `host.register(this)`, and on `dispose()` it calls `host.unregister(this)`. A side panel button exports the chain to XLSX.

The control does not subscribe to data and does not send orders: the chain and the context are handed to it by the host through `update`.

## Public methods

- `OptionDeskWidget.create(hostEl, state, deps)` — build the panel and add it to the container.
- `update(strikes, context)` — replace the chain and the valuation context.
- `rows()` — return the rows the way the desk stores them: with computed bar scales and the intrinsic value.
- `dispose()` — release resources and unregister from the host.
- `OptionDeskWidget.TYPE` — the control type identifier, `ControlTypes.OptionDesk`.

## Exported functions

The computational part is available separately from the panel:

- `scaleChain(strikes, context)` — computes the bar maximums and the intrinsic value of every strike in one pass over the chain.
- `sideGreeks(row, which, context)` — the greeks of one strike side: either supplied by the host or computed from its volatility; `null` when neither is possible.
- `greekPlaces(values)` — the number of decimals for a column of values.
- `greekScales(rows, context)` — the number of decimals for each greek, measured across both sides of the chain at once.

## See also

- [JavaScript Trading Controls](../trading_controls.md)
- [Watchlist](watchlist.md)
- [Order book](order_book.md)
- [Positions](positions.md)
