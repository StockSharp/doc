# Auftragseingabe

`OrderEntryWidget` ist ein zweiseitiges Kauf- und Verkaufspanel. Es unterstützt Market-, Limit-, Stop- und Stop-Limit-Aufträge, zeigt nur die zum gewählten Typ gehörenden Felder an und prüft die Werte vor der Übergabe an den Host.

## Erstellen und Instrument konfigurieren

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
      console.log('senden', side, values),
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

`OrderEntryTypes` enthält `Market`, `Limit`, `Stop` und `StopLimit`, `OrderEntrySides` enthält `Buy` und `Sell`.

## Prüfen und senden

Das Steuerelement prüft positive Werte, Losgröße, Preisschritt sowie Mindest- und Höchstvolumen. Optionales Take Profit und Stop Loss werden mit der Methode `toggleTpSl` aktiviert.

`submit(side)` ruft zunächst `validate(side)` auf. Bei gültigen Daten erhält der Handler `submitOrder` die Seite und folgendes Objekt:

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

Das Steuerelement selbst wählt kein Portfolio aus, prüft nicht die Verbindung und sendet keinen Auftrag an den Server. Diese Aufgaben bleiben dem Host-Handler überlassen.

## Preise, Volumen und Zustand

- `setBbo(bid, ask)` aktualisiert den besten Kauf- und Verkaufskurs.
- `applyBbo(side)` trägt auf der gewählten Seite die gegenüberliegende BBO-Seite zur sofortigen Ausführung ein.
- `setAvailable(side, value)` legt den angezeigten verfügbaren Saldo fest.
- `setMaxQuantity(side, value)` legt den Höchstwert fest, aus dem die Prozentschaltflächen die Menge berechnen.
- `applyPercent(side, pct)` wendet 25, 50, 75 oder 100 Prozent des mit `setMaxQuantity` festgelegten Höchstwerts an.
- `preselect(side)` wählt nach der Geste „Handel per Klick“ die Seite vor.
- `setEnabled(false)` verhindert das Senden, ohne eingegebene Werte zu löschen.
- `getValues(side)` und `validate(side)` ermöglichen die Prüfung des Formulars von außen.

Die statische Methode `toApiType` konvertiert `Limit` in `0`, `Market` in `1` sowie `Stop` und `StopLimit` in `2`.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../javascript_trading_controls.md)
- [Orderbuch](order_book.md)
- [Positionen](positions.md)
