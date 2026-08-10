# Ausführungshistorie

`TradeHistoryWidget` ist eine Tabelle mit den Ausführungen des aktuellen Portfolios. Die neuesten Ausführungen stehen oben; jede Zeile zeigt Zeit, Instrument, Seite, Menge, Preis, Ausführungskennung und Auftragskennung.

## Erstellen und laden

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

Beim Erstellen des Steuerelements wird der Ladevorgang nicht automatisch gestartet. Die Methode `refresh()` ermittelt das aktuelle Portfolio über `host.trading.portfolioId()`, prüft die Berechtigung und lädt anschließend die Ausführungen:

```ts
const portfolioId = host.trading.portfolioId();
if (portfolioId && host.allow('load trade history'))
  host.trading.api.getExecutions(portfolioId, null, 200);
```

Dieser Ansatz ist beim Wechsel zwischen Portfolios wichtig: Die Kennung wird unmittelbar vor jeder Aktualisierung gelesen und nicht beim Erstellen des Panels gespeichert.

## Verhalten

Das Panel ist schreibgeschützt. Benutzer können Zeilen sortieren und auswählen, das Kontextmenü öffnen, die Daten aktualisieren und die sichtbaren Spalten nach XLSX exportieren. Ladefehler werden an `host.log` übergeben.

Die öffentliche API des Steuerelements besteht aus `refresh(): Promise<void>` und `dispose(): void`.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../javascript_trading_controls.md)
- [Aktive Aufträge](active_orders.md)
- [Handelsstrom](trade_feed.md)
