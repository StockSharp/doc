# Handel aus dem Chart

`TradingLayer` ist die brokerunabhängige Schicht des Handelszustands eines Charts: Sie hält normalisierte Aufträge, Positionen, Ausführungen und die Quotierung und gibt Benutzeraktionen als Absichten (`TradingIntent`) nach außen. Die Schicht sendet nichts selbst — sie enthält weder Transport noch Konto noch Wiederholungen, die Verbindung zum Broker bleibt beim Host.

![Auftragslinien, Position und Schutzaufträge über dem Chart](../../../../images/javascript_charts_trading.png)

## Einbinden

Die Schicht wird als eigener Einstiegspunkt des Pakets [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) ausgeliefert:

```ts
import { TradingLayer, TradingLayerPrimitive } from '@stocksharp/chart/trading';
```

Ohne Bundler stehen dieselben Klassen im globalen Objekt `SSChart` bereit (`new SSChart.TradingLayer({ tickSize: 0.25 })`).

## Erstellen und aktualisieren

Die Schicht wird mit den Parametern des Kursrasters erstellt, das Primitiv `TradingLayerPrimitive` zeichnet ihren Zustand über die Serie, und `TradingOrderPlacementAdapter` verwandelt einen Klick auf das Chart in die Absicht, einen Auftrag zu platzieren:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  TradingLayer,
  TradingLayerPrimitive,
  TradingOrderPlacementAdapter,
  type TradingIntent,
} from '@stocksharp/chart/trading';

declare const broker: { send(intent: TradingIntent): Promise<void> };

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {
  timeScale: { timeVisible: true },
});
const candles = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const layer = new TradingLayer({ tickSize: 0.25 });

chart.attachPrimitive(new TradingLayerPrimitive(layer, { showInactiveOrders: false }), {
  series: candles,
});

// Der kanonische Zustand des Brokers: Das Chart zeigt ihn nur an und ändert ihn niemals selbst.
layer.setOrders([{
  id: 'ob1',
  side: 'buy',
  type: 'limit',
  status: 'working',
  timeInForce: 'good-till-cancelled',
  quantity: 2,
  filledQuantity: 0,
  price: 96,
  revision: 1,
  permissions: { canModify: true, canCancel: true },
  label: 'BID',
}]);

layer.setPositions([{
  id: 'pos1',
  side: 'long',
  quantity: 3,
  averagePrice: 100,
  revision: 1,
  pnl: { realized: 0, unrealized: 45, currency: 'USD', markPrice: 101.5 },
  permissions: { canClose: true, canReverse: true, canProtect: true },
}]);

layer.setQuote({ time: 1704326400, bidPrice: 100.75, bidSize: 5, askPrice: 101, askSize: 4 });

// Absichten gehen an den Host, der Host antwortet der Schicht mit dem Ergebnis.
layer.subscribeIntents(intent => {
  broker.send(intent).then(
    () => layer.resolveIntent({ intentId: intent.intentId, status: 'accepted' }),
    (error: Error) => layer.resolveIntent({
      intentId: intent.intentId,
      status: 'rejected',
      reason: error.message,
    }),
  );
});

// Strg + linke Taste — Limitkauf, Strg + rechte Taste — Limitverkauf.
new TradingOrderPlacementAdapter(chart, layer, { quantity: 1, orderType: 'limit', modifier: 'ctrl' });
```

`setOrders`, `setPositions`, `setExecutions` und `setQuote` ersetzen die jeweilige Sammlung vollständig. Jeder Aufruf wird normalisiert und mit dem aktuellen Zustand verglichen: Hat sich nichts geändert, werden die Abonnenten nicht aufgerufen, andernfalls wird eine Änderung mit den Feldern `added`, `updated` (Paare aus `previous` / `current`), `removed` und `orderChanged` veröffentlicht. Die Quotierung ist die Ausnahme: Ihre Änderung hat nur `previous` und `current`. Den aktuellen Ausschnitt liefert `state()` — das ist `{ version, orders, positions, executions, quote }`.

Die Normalisierung ist streng: Kurse müssen auf dem Raster `tickSize` (mit dem Versatz `priceOrigin`) liegen, Mengen auf `quantityStep`, sofern dieser gesetzt ist; ein Limitauftrag verlangt `price`, ein Stoppauftrag `stopPrice` und ein Stop-Limit-Auftrag beide Felder. Inkonsistente Daten werden mit einer Ausnahme abgelehnt und nicht stillschweigend korrigiert.

## Absichten

Benutzeraktionen führt die Schicht nicht aus, sondern veröffentlicht sie als Absicht: Eine `request*`-Methode gibt das Absichtsobjekt zurück, legt es in die Warteschlange der offenen Absichten und übergibt es den Abonnenten von `subscribeIntents`. Der Host führt die Absicht beim Broker aus und schließt sie mit dem Aufruf `resolveIntent({ intentId, status, reason })` mit dem Status `accepted` oder `rejected` ab — das Ergebnis trifft zusammen mit der ursprünglichen Absicht in `subscribeIntentOutcomes` ein. Die offenen Absichten listet `pendingIntents()` auf.

Die Rechte werden vor der Veröffentlichung geprüft: Das Ändern eines Auftrags erfordert `permissions.canModify`, das Stornieren `canCancel`, Aktionen an einer Position `canClose`, `canReverse` und `canProtect`. Ein Auftrag ohne Rechte gilt als schreibgeschützt, und der Aufruf löst eine Ausnahme aus. In die Absicht wird `expectedRevision` aus der kanonischen Entität eingesetzt, damit der Broker eine auf veralteten Daten aufgebaute Anfrage ablehnen kann.

## Zeichnen und Ziehen

`TradingLayerPrimitive` ist ein Chart-Primitiv, das die Schicht beim Anhängen abonniert und Auftragslinien mit Beschriftungen, die Positionslinie mit P&L, Ausführungsmarkierungen, Bid-/Ask-/Last-Linien und die Verbindungen der Brackets zeichnet. Was angezeigt wird und in welchen Farben, legen die Optionen des Konstruktors und `applyOptions` fest: `showOrders`, `showInactiveOrders`, `showPositions`, `showExecutions`, `showExecutionLabels`, `showQuote`, `showPnl`, `showBrackets`, `autoscale`, der Farbsatz (`orderBuyColor`, `orderSellColor`, `inactiveOrderColor`, `longPositionColor`, `shortPositionColor`, `executionBuyColor`, `executionSellColor`, `bidColor`, `askColor`, `lastColor`, `bracketColor`), `lineWidth`, `fontSize`, `orderLabelSpacing`, `zOrder` sowie die Formatierer `priceFormatter`, `quantityFormatter` und `pnlFormatter`. Die Kennung `id` wird einmal beim Erstellen gesetzt und später nicht mehr geändert.

Eine Auftragslinie lässt sich mit der Maus ziehen, wenn der Auftrag aktiv (`pending`, `working` oder `partially-filled`) und kein Marktauftrag ist und das Recht `canModify` besitzt. Während des Ziehens zeigt das Primitiv einen an das Raster gebundenen Vorschaukurs; beim Loslassen der Taste wird die Absicht `requestModifyOrder` veröffentlicht, und die Vorschau bleibt bestehen, bis der Host die Absicht abschließt — eine abgelehnte bringt die Linie auf den kanonischen Kurs zurück.

Den Treffer des Cursors beschreiben `TradingOrderHitData`, `TradingPositionHitData`, `TradingExecutionHitData` und `TradingQuoteHitData`; sie im Handler zu erkennen hilft `isTradingPrimitiveHitData(value)`.

## Aufträge mit der Maus platzieren

`TradingOrderPlacementAdapter` verbindet das reguläre Platzierungssignal des Charts mit der Schicht: Er schaltet den Platzierungsmodus ein, hört auf Klicks und ruft `requestPlaceOrder` auf. Er erzeugt selbst weder Kurslinien noch spricht er mit dem Broker.

Die Optionen: `quantity` (erforderlich), `orderType` — `'limit'` oder `'stop'` (standardmäßig `'limit'`), `timeInForce` (standardmäßig `'good-till-cancelled'`), `modifier` — `'ctrl'`, `'shift'` oder `'alt'` (standardmäßig `'ctrl'`), `color`, `title`, `enabled` und `sideResolver`. Der Standard-Resolver liefert Kauf bei der linken und Verkauf bei der rechten Taste, und `null` bricht die Platzierung ab. Gesteuert wird der Adapter über die Methoden `options()`, `applyOptions(patch)`, `setEnabled(enabled)` und `dispose()`.

## Öffentliche Methoden der Schicht

- `setOrders(orders)`, `setPositions(positions)`, `setExecutions(executions)`, `setQuote(quote)` — den kanonischen Zustand ersetzen; `setQuote(null)` entfernt die Quotierung.
- `state()` — der aktuelle Zustandsausschnitt mit Versionsnummer.
- `normalizationOptions()` — die Parameter des Kursrasters, mit denen die Schicht erstellt wurde.
- `subscribeChanges(handler)`, `subscribeIntents(handler)`, `subscribeIntentOutcomes(handler)` — Abonnements; jedes gibt eine Funktion zum Abbestellen zurück.
- `pendingIntents()`, `resolveIntent(resolution)` — die Warteschlange der offenen Absichten und ihr Abschluss.
- `requestPlaceOrder(order)`, `requestModifyOrder(orderId, changes)`, `requestCancelOrder(orderId)` — Arbeit mit Aufträgen.
- `requestClosePosition(positionId, quantity)`, `requestReversePosition(positionId, quantity)` — Arbeit mit einer Position; ohne Menge wird die gesamte Position genommen.
- `requestCreateStopLoss`, `requestEditStopLoss`, `requestRemoveStopLoss`, `requestCreateTakeProfit`, `requestEditTakeProfit`, `requestRemoveTakeProfit` — die Schutzaufträge eines Brackets.
- `dispose()` — Ressourcen freigeben.

## Die übrigen Exporte

- Aufzählungen des Modells: `TradingSide`, `ChartOrderType`, `ChartOrderStatus`, `ChartOrderTimeInForce`, `ChartPositionSide`, `ChartBracketRole`, `ChartExecutionLiquidity`, `TradingIntentKind`.
- Aufzählungen der Schicht und des Primitivs: `TradingLayerChangeKind`, `TradingIntentOutcomeStatus`, `TradingPrimitiveEntityKind`, `TradingQuoteKind`.
- Prüfung und Normalisierung der Daten: `normalizeChartOrder`, `normalizeChartOrders`, `normalizeChartPosition`, `normalizeChartPositions`, `normalizeChartExecution`, `normalizeChartExecutions`, `normalizeChartQuote`, `normalizeChartOrderRequest`, `normalizeTradingIntent`, `normalizeTradingModelOptions`.
- Hilfsberechnungen: `quantizeTradingPrice` — die Bindung eines beliebigen Kurses an das Raster, `chartOrderRemainingQuantity` — der nicht ausgeführte Rest eines Auftrags, `chartPnlTotal` — die Summe aus realisiertem und nicht realisiertem P&L.
- Die Typen `ChartOrder`, `ChartPosition`, `ChartExecution`, `ChartQuote`, `ChartOrderRequest`, `ChartOrderModification`, `TradingIntent` und die davon abgeleiteten Absichts-Schnittstellen.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Kerzenchart](candlestick.md)
- [Nachladen der Historie](backfill.md)
