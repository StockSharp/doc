# Aktive Aufträge

`ActiveOrdersWidget` zeigt die vollständige Auftragsliste mit dem jeweiligen aktuellen Status. Ausgeführte, stornierte und abgelehnte Zeilen bleiben in der Tabelle, sodass Benutzer die gesamte Abfolge der Änderungen innerhalb einer Sitzung sehen.

## Erstellung

```ts
import {
  ActiveOrdersWidget,
  OrderStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let orders!: ActiveOrdersWidget;

orders = ActiveOrdersWidget.create(
  document.querySelector<HTMLElement>('#orders')!,
  {},
  {
    host,
    cancelOrder: id => console.log('stornieren', id),
    dismissOrder: id => orders.removeOrder(id),
    editOrderField: (id, field) => {
      if (field === 'quantity' || field === 'limitPrice' || field === 'stopPrice')
        orders.startInlineEdit(id, field);
    },
    replaceOrder: (id, quantity, limitPrice, stopPrice) =>
      console.log('ersetzen', id, quantity, limitPrice, stopPrice),
    cancelAllOrders: () => console.log('alle stornieren'),
    refreshOrders: () => orders.update([]),
  },
);

orders.update([{
  id: 501,
  localId: 1,
  instrument: 'BTC@IMEX',
  side: 0,
  type: 0,
  quantity: 0.01,
  limitPrice: 68_400,
  status: OrderStates.Active,
}]);
```

`update` ersetzt den gesamten Zeilensatz. Verwenden Sie für laufende Änderungen `applyDelta(order)` und zum Entfernen einer einzelnen Zeile `removeOrder(orderId)`.

## Bearbeitung und Aktionen

Die Menge kann per Doppelklick geändert werden, solange sich der Auftrag im Status `Sent` oder `Active` befindet. Der Limitpreis lässt sich nur bearbeiten, wenn `limitPrice` bereits größer als `0` ist; für den Stopppreis gilt entsprechend, dass `stopPrice` bereits größer als `0` sein muss. Nach der Bestätigung ruft das Steuerelement `replaceOrder` auf und übergibt das vollständige Tripel `quantity`, `limitPrice`, `stopPrice`, nicht nur das geänderte Feld.

Bei einem aktiven Auftrag ruft die Aktionsschaltfläche `cancelOrder` auf. Bei einer abgeschlossenen Zeile ruft sie `dismissOrder` auf und entfernt den Eintrag nur aus der lokalen Ansicht. Der Ablehnungsgrund `rejectReason` wird in einem Tooltip angezeigt.

Das Steuerelement unterstützt außerdem das Stornieren aller Aufträge, Aktualisieren, Sortieren, Markieren von Zeilen, ein Kontextmenü und den Export nach XLSX.

## Öffentliche Methoden

- `update(orders)` — alle Zeilen ersetzen.
- `applyDelta(order)` — einen Auftrag hinzufügen oder aktualisieren.
- `removeOrder(orderId)` — eine Zeile entfernen.
- `getOrder(orderId)` — die aktuelle Zeile abrufen.
- `startInlineEdit(orderId, field)` — die Bearbeitung von `quantity`, `limitPrice` oder `stopPrice` beginnen.
- `dispose()` — die Ressourcen des Steuerelements freigeben.

Das Objekt `OrderStates` exportiert die Statuswerte `PendingRisk`, `Sent`, `Active`, `PartiallyFilled`, `Filled`, `Rejected` und `Cancelled`.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../javascript_trading_controls.md)
- [Positionen](positions.md)
- [Ausführungshistorie](trade_history.md)
