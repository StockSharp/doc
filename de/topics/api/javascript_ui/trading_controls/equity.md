# Equity-Kurve

`EquityWidget` zeichnet den kumulierten P&L eines Laufs als Diagramm über die Zeit. Das Panel ist unter der Kennung `equity` (`ControlTypes.Equity`) registriert, und die Kurve selbst baut die Engine `@stocksharp/chart` auf, die als Peer-Abhängigkeit eingebunden wird.

![Equity-Kurve aus den Ergebnissen eines Laufs](../../../../images/javascript_controls_equity.png)

## Erstellen und aktualisieren

```ts
import {
  EquityWidget,
  type PnlPoint,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const equity = EquityWidget.create(
  document.querySelector<HTMLElement>('#equity')!,
  {},
  { host },
);

const start = Date.now() - 3 * 60 * 60 * 1000;

const points: PnlPoint[] = [
  { time: start, value: 0 },
  { time: start + 45 * 60 * 1000, value: 320.5 },
  { time: start + 95 * 60 * 1000, value: -140.25 },
  { time: Date.now(), value: 1_180.75 },
];

equity.update(points);
```

Die einzige Abhängigkeit von `EquityDeps` ist `host`; Aktionshandler hat das Panel nicht, weil es auf der Kurve nichts auszuführen gibt. Das zweite Argument von `create` — der gespeicherte Instanzzustand — wird vom Steuerelement nicht verwendet: Eigene Einstellungen speichert das Panel weder im Zustand noch in `host.preferences`.

`update` ersetzt den Lauf vollständig. Der Wert der Kurve ist kumulativ, deshalb gilt eine Punktmenge ohne die zuvor übergebenen Stichproben als anderer Lauf und nicht als Fortsetzung des bisherigen.

## Daten und Darstellung

Das Feld `time` in `PnlPoint` sind Unix-Millisekunden; die Engine rechnet die Zeit in Sekunden, die Umrechnung nimmt das Steuerelement selbst vor. Die Punkte werden nach Zeit sortiert, Stichproben mit nicht numerischem `time` oder `value` werden verworfen, und von mehreren Werten innerhalb derselben Sekunde bleibt der letzte übrig — er zeigt, wo der Lauf zu diesem Zeitpunkt tatsächlich stand.

Solange weniger als zwei brauchbare Punkte vorliegen, zeigt das Panel die Meldung zum Schlüssel `NoEquity`, das Diagramm wird nicht erzeugt, und `chart()` gibt `null` zurück. Das Diagramm selbst entsteht beim ersten Zeichnen und nicht im Konstruktor: Die Engine benötigt einen bereits auf der Seite platzierten Container. Die Größenänderung des Bereichs verfolgt ein `ResizeObserver`, der `resize` der Engine aufruft.

Farben, Schrift und Gitterfarbe stammen aus `host.presentation.canvasPalette()`. Die Kurve wird nach dem letzten Wert des Laufs eingefärbt: `up` bei einem Wert von mindestens null und `down` bei einem negativen; die Füllung unter der Linie ist dieselbe Farbe, oben auf 28 % und unten auf 2 % abgeschwächt. Die Zeitachse zeigt Stunden und Sekunden in der Zeitzone des Browsers und, wenn `Intl` nicht verfügbar ist, in UTC.

## Titelleiste, Tooltip und Schaltflächen

In der Kopfzeile des Panels wird der letzte Wert des Laufs im Format `formatPnl` ausgegeben — mit Vorzeichen und zwei Nachkommastellen; die Farbklasse liefert `host.presentation.pnlClass`. Über der Kurve wird der Wert unter dem Zeiger angezeigt: der Zeitpunkt in Worten über `host.presentation.timeText`, daneben der Wert selbst. Verlässt der Zeiger das Diagramm, wird der Tooltip geleert.

Die Schaltfläche in der Kopfzeile mit dem Tooltip `ResetView` ruft `resetZoom` auf, die Schaltfläche zum Schließen `host.close()`. Den sichtbaren Text bezieht das Panel über die Schlüssel `Equity`, `ResetView`, `ClosePanel`, `PnLChart` und `NoEquity`.

Für die Form der Kurve, ihre Farbe und den Maßstab ist das Steuerelement selbst zuständig. Dem Host bleiben die Sprache der Beschriftungen, die Palette, das Format des Zeitpunkts und die Quelle der Punkte selbst: Der P&L wird vom Steuerelement nicht berechnet, und Daten fordert es nirgendwo an.

## Öffentliche Methoden

- `EquityWidget.create(hostEl, state, deps)` — das Panel erstellen und sein Wurzelelement in den Container einfügen.
- `EquityWidget.TYPE` — die Kennung `equity`.
- `update(points)` — den Lauf vollständig anzeigen.
- `resetZoom()` — nach dem Zoomen zur Übersicht über den gesamten Lauf zurückkehren.
- `chart()` — die `IChartApi`-Instanz, damit der Host Eigenes ergänzt: eine Benchmark-Linie, eine Drawdown-Markierung. Bis das Diagramm entsteht, wird `null` zurückgegeben.
- `dispose()` — den Größenbeobachter abschalten, das Diagramm entfernen und die Registrierung im Host aufheben.

Die Eigenschaft `rootEl` liefert das Wurzelelement des Panels.

## Die Chart-Engine einbinden

Das Paket `@stocksharp/chart` wird separat installiert:

```bash
npm install @stocksharp/chart
```

Das fertige Bundle `sstradingcontrols.js` enthält die Engine nicht, deshalb werden ihre Skripte auf einer Seite ohne Bundler daneben eingebunden:

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/indicators/dist/ssindicators.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/chart/dist/sschart.js"></script>
```

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Positionen](positions.md)
- [Ausführungshistorie](trade_history.md)
- [JavaScript-Charts](../charts.md)
