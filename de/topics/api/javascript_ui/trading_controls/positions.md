# Positionen

`PositionsWidget` zeigt offene Positionen und eine oben angeheftete Zeile mit dem Geldsaldo an. Positionen sind standardmäßig alphabetisch geordnet; der Saldo wird bei Sortierung, Auswahl und Export nicht berücksichtigt.

## Erstellen und aktualisieren

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
      console.log('schließen', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('umkehren', portfolioId, instrumentId, symbol),
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

`update` ersetzt die Positionsliste. `applyDelta` aktualisiert eine Position anhand der Kombination aus Portfolio und Instrument; eine Zeile mit einer Menge von null wird entfernt. `updateBalance(null)` entfernt den angehefteten Saldo.

## Daten und Aktionen

Die Zeile zeigt Menge, Durchschnitts- und aktuellen Preis sowie einen Gesamt-PnL an, der als Summe aus `realizedPnl` und `unrealizedPnl` berechnet wird. Die zugehörige Farbklasse liefert `host.presentation.pnlClass`.

Die Schaltflächen einer Zeile rufen die vom Host übergebenen Funktionen `closePosition` und `reversePosition` auf. Das Steuerelement erstellt und sendet selbst keine Handelsaufträge.

## Öffentliche Methoden

- `update(positions)` — alle Positionen ersetzen.
- `updateBalance(balance)` — den Geldsaldo setzen oder entfernen.
- `applyDelta(position)` — die laufende Änderung einer Position anwenden.
- `dispose()` — Ressourcen freigeben.

Das Panel unterstützt außerdem Aktualisierung, Sortierung, Kontextmenü und den Export der Positionen nach XLSX.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Aktive Aufträge](active_orders.md)
- [Auftragseingabe](order_entry.md)
