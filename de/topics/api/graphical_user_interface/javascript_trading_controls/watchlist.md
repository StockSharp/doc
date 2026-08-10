# Instrumentenliste

`WatchlistWidget` zeigt Instrumente und laufende Kurse an. Das Steuerelement unterstützt Suche, Favoriten, Kategorien, Auswahl des aktiven Instruments und Abonnements ausschließlich für die tatsächlich sichtbaren Symbole.

## Erstellung

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
      console.log('ausgewählt', symbol);
    },
  },
);
```

Beim Erstellen wird automatisch `init()` gestartet. Die Methode lädt die Liste über `host.trading.api.searchInstruments('')`. Für jeden Eintrag werden die Felder `symbol`, `name`, `exchange` und `category` verwendet.

Der Kursstrom wird von außen übergeben:

```ts
watchlist.onPriceUpdate('BTC@IMEX', 68_420.5);
```

Die Methode aktualisiert nur die erforderlichen Preis- und Prozentzellen und behält die Änderungsanimation bei. `setCurrentSymbol(symbol)` hebt das aktuelle Instrument hervor.

## Suche, Kategorien und Abonnements

Die Suche prüft Symbol, Name und Börse. Registerkarten werden für alle Instrumente, Favoriten und erkannte Kategorien erstellt. Favorisierte Symbole werden in `host.preferences` gespeichert.

Das Steuerelement abonniert die ersten 30 sichtbaren Instrumente auf der Ebene `MarketDataLevels.Quotes`. Auf dem Bildschirm werden höchstens 300 Zeilen gerendert, Filterung und Export arbeiten jedoch mit der gesamten Ergebnismenge. Nur eine Instanz mit `host.isPrimary === true` veröffentlicht sichtbare Kurse über `host.ticker`.

Die prozentuale Änderung wird anhand des ersten empfangenen Preises des aktuellen UTC-Tages berechnet. Diese Basiskurse gehören zum Cache und werden deshalb in `host.cache`, nicht in den Benutzereinstellungen gespeichert.

## Öffentliche Methoden

- `init()` — Instrumente laden und Abonnements vorbereiten; wird bei `create` automatisch aufgerufen.
- `setCurrentSymbol(symbol)` — das aktive Instrument markieren.
- `onPriceUpdate(symbol, price)` — einen neuen Preis anwenden.
- `dispose()` — Abonnements entfernen und Ressourcen freigeben.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../javascript_trading_controls.md)
- [Orderbuch](order_book.md)
- [Auftragseingabe](order_entry.md)
