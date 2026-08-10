# Handelsstrom

`TradeFeedWidget` zeigt öffentliche Marktausführungen und Ausführungen des aktuellen Portfolios. Der Handelsstrom kann zwischen einer Tabelle und einem Bubble-Chart umgeschaltet werden.

## Erstellung und Datenstrom

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

`setTrades` ersetzt den Ausgangsdatensatz, `addTrade` fügt eine einzelne laufende Ausführung hinzu. Das Widget hält höchstens 50 Tabellenzeilen und die letzten 500 Ticks für den Chart vor.

## Bubble-Chart

Im Chart stellt die horizontale Achse die Zeit, die vertikale Achse den Preis, der Radius einer Blase das Volumen und ihre Farbe die Ausführungsseite dar. Werden benachbarte Ticks zusammengefasst, zeigt der Tooltip VWAP, Gesamtvolumen und Anzahl der Ausführungen. Eine Ausführung gilt als groß, wenn ihr Volumen den gleitenden Durchschnitt um mehr als das Doppelte übersteigt.

Die Canvas-Farben stammen aus `host.presentation.canvasPalette()`. Der ausgewählte Modus wird in `host.preferences` unter einem seitenweit gemeinsamen Schlüssel gespeichert.

## Zusätzliche Instrumente

Neben dem aktiven Symbol kann das Panel weitere Symbole anheften:

```ts
await feed.addExtraSymbol('ETH@IMEX');
console.log(feed.getExtraSymbols());
await feed.removeExtraSymbol('ETH@IMEX');
```

Für diese Instrumente werden Abonnements der Ebene `MarketDataLevels.Tape` erstellt; im Bubble-Chart erhalten sie separate Bahnen mit jeweils eigener Preisskala. Die Liste zusätzlicher Instrumente wird im Zustand der jeweiligen Instanz gespeichert und kann beim Erstellen als `{ extras: ['ETH@IMEX'] }` übergeben werden.

## Eigene Ausführungen

Die zweite Registerkarte zeigt Ausführungen des Portfolios. Das Laden kann ausdrücklich angestoßen werden:

```ts
await feed.loadMyTrades(host.trading.portfolioId(), 'BTC@IMEX');
```

Die Methode ruft `host.trading.api.getExecutions` auf. Der übrige Handelsstrom wird dem Steuerelement stets von außen übergeben, damit mehrere Panels dieselbe Verbindung verwenden können.

## Öffentliche Methoden

- `setActiveSymbol(symbol)` — das Hauptinstrument festlegen.
- `setTrades(...)`, `addTrade(...)` — den Handelsstrom ersetzen oder ergänzen.
- `loadMyTrades(portfolioId, symbol)` — eigene Ausführungen laden.
- `addExtraSymbol`, `removeExtraSymbol`, `getExtraSymbols` — angeheftete Instrumente verwalten.
- `dispose()` — Abonnements entfernen und Ressourcen freigeben.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../javascript_trading_controls.md)
- [Ausführungshistorie](trade_history.md)
- [Orderbuch](order_book.md)
