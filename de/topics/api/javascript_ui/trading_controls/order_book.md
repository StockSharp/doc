# Orderbuch

`OrderBookWidget` zeigt Bid- und Ask-Level, Mittelpreis, Spread, kumuliertes Volumen und Marktstimmung an. Zur Verfügung stehen diagonale und gestapelte Ansichten, Seitenumkehrung, eine Tiefe von 5 oder 10 Leveln sowie ein Canvas-Tiefenchart.

## Erstellung

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
      console.log('vorbelegen', price, side),
    onPriceExecuted: (price, side) =>
      console.log('ausführen', price, side),
    maxDepth: () => 10,
    pixelRatio: () => window.devicePixelRatio || 1,
  },
);

book.setSymbol('BTC@IMEX');
```

Ein normaler Klick auf ein Level ruft `onPriceSelected` auf, ein Klick mit Strg oder Cmd dagegen `onPriceExecuted`. Die numerische Seite `0` bedeutet Kauf und `1` Verkauf: Ein Klick auf Ask wählt Kauf, ein Klick auf Bid Verkauf. Das Steuerelement übergibt die Absicht an den Host, sendet jedoch selbst keinen Auftrag.

## Momentaufnahmen und Änderungen des Orderbuchs

Der erste Frame muss eine vollständige Momentaufnahme sein:

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

Nachfolgende Frames mit `isSnapshot: false` werden als Änderungen angewendet. Eine Menge von `0` entfernt das Level. `sequence` muss ohne Lücken ansteigen; bei einer Lücke ruft das Steuerelement `host.trading.marketData.resubscribe(symbol, MarketDataLevels.Full)` auf, um eine neue Momentaufnahme abzurufen. Ungültige Level und ein gekreuztes Orderbuch werden an `host.log` übergeben.

## Ansicht und Zustand

```ts
book.setDepth(10);
book.setView('stacked');
book.setInvertSides(false);
book.setShowDepthChart(true);
```

Mit `maxDepth()` kann der Host die Zahl der Level für einen kleinen Bildschirm reduzieren; `pixelRatio()` legt die Dichte des Canvas-Backing-Stores fest. Die Chartfarben liefert `host.presentation.canvasPalette()`.

Bei einem Panel, das dem aktiven Instrument folgt, werden die Ansichtseinstellungen in `host.preferences` gespeichert. Eine angeheftete Instanz speichert sie im Panelzustand. Beim Erstellen können `symbol`, `depth`, `view`, `invertSides`, `showDepthChart` und `followsActive` übergeben werden.

Aktive Aufträge aus `host.trading.marketData.getOrders()` werden neben den entsprechenden Leveln markiert.

## Öffentliche Methoden

- `setSymbol`, `getSymbol` — verwalten das Instrument.
- `setDepth`, `getDepth` — setzen die Tiefe und geben sie zurück.
- `setView`, `setInvertSides`, `setShowDepthChart` — ändern die Ansicht.
- `getBids`, `getAsks` — geben die aktuellen Level zurück.
- `isFollowsActive` — meldet, ob das Panel dem aktiven Instrument folgt.
- `applyFrame` — wendet eine Momentaufnahme oder eine Änderung an.
- `dispose` — gibt Ressourcen frei.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Instrumentenliste](watchlist.md)
- [Auftragseingabe](order_entry.md)
