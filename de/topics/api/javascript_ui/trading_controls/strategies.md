# Strategien

`StrategiesWidget` zeigt die Liste der laufenden Strategien an: eine Zeile je Strategie mit ihrem Zustand, dem Handelsmodus, der Position, den Auftrags- und Trade-Zählern, dem Gewinn und den Steuerschaltflächen. Die Kennung des Steuerelements ist `strategies` (`ControlTypes.Strategies`), ebenfalls verfügbar über die statische Eigenschaft `StrategiesWidget.TYPE`.

![Strategieliste mit Zustand, Position, PnL und Ertragskurve](../../../../images/javascript_controls_strategies.png)

## Erstellen und aktualisieren

```ts
import {
  StrategiesWidget,
  StrategyStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let strategies!: StrategiesWidget;

strategies = StrategiesWidget.create(
  document.querySelector<HTMLElement>('#strategies')!,
  {},
  {
    host,
    tradingModes: ['Disabled', 'CancelOrders', 'ReducePosition', 'Full'],
    start: id => console.log('start', id),
    stop: id => console.log('stop', id),
    closePosition: id => console.log('flatten', id),
    openStrategy: id => console.log('open', id),
    riskRules: id => console.log('risk', id),
    setTradingMode: (id, mode) => console.log('mode', id, mode),
  },
);

strategies.update([{
  id: 'sma-1',
  name: 'SMA crossover',
  state: StrategyStates.Started,
  online: true,
  tradingMode: 'Full',
  portfolio: 'Demo',
  security: 'BTC@IMEX',
  position: 0.25,
  ordersCount: 12,
  tradesCount: 8,
  pnlChange: 105,
  realized: 20,
  unrealized: 85,
  pnl: [
    { time: 1, value: 0 },
    { time: 2, value: 60 },
    { time: 3, value: 105 },
  ],
}]);
```

`update` nimmt den vollständigen Zeilensatz entgegen und ersetzt die Tabelle damit: Eine Strategie, die in der übergebenen Liste fehlt, gilt als entfernt, und ihre Zeile verschwindet. Ein fortlaufendes Aktualisieren einer einzelnen Zeile bietet das Steuerelement nicht — der neue Zustand kommt als vollständige Liste.

Das zweite Argument von `create` ist der gespeicherte Instanzzustand. Das Steuerelement liest ihn nicht und speichert selbst nichts: weder Schlüssel in `host.preferences` noch Aufrufe von `host.persistState`.

Das Objekt `StrategyStates` exportiert die Zustände `Stopped`, `Starting`, `Started` und `Stopping`.

## Abhängigkeiten

Erforderlich ist nur der Host. Dessen Vollständigkeit prüft beim Erstellen die Funktion `assertHost`, deshalb führt ein unvollständiger `TradingHost` zu einer Ausnahme mit dem Namen des fehlenden Mitglieds und nicht zu einer wirkungslosen Schaltfläche.

| Abhängigkeit | Erforderlich | Standardverhalten |
|---|---|---|
| `host` | erforderlich | — |
| `start(id)` | optional | Die Start-Schaltfläche wird nicht erzeugt. |
| `stop(id)` | optional | Die Stopp-Schaltfläche wird nicht erzeugt. |
| `closePosition(id)` | optional | In der Positionsspalte bleibt nur die Zahl. |
| `openStrategy(id)` | optional | Die Schaltfläche zum Wechsel zur Strategie wird nicht erzeugt. |
| `riskRules(id)` | optional | Die Schaltfläche für die Risikoregeln wird nicht erzeugt. |
| `setTradingMode(id, mode)` | optional | Der Handelsmodus wird als Text angezeigt. |
| `tradingModes` | optional | Leere Liste, die Auswahlliste der Modi wird nicht erzeugt. |

Ein solcher Satz erlaubt es, ein reines Lese-Panel zusammenzustellen: Wird keine einzige Aktionsfunktion übergeben, zeigt die Tabelle Daten und keine einzige Schaltfläche.

Die Zeichenfolgen in `tradingModes` laufen durch `host.t`, dienen also als Übersetzungsschlüssel. Sie gehören dem Host und nicht dem Paket, deshalb ist ihr Fehlen in `translation-keys.json` normal, und übersetzt werden sie vom Host.

## Zustände und Aktionen

Die Zustandszelle besteht aus einem Punkt und einem Wort: Der Punkt wird beim schnellen Überfliegen der Liste gelesen, das Wort unterscheidet `Starting` von `Started`. Ist in der Zeile das Feld `error` gefüllt, gelangt der Fehlertext in den Tooltip sowohl des Punktes als auch des Wortes, und eine wegen einer Störung gestoppte Strategie wird mit dem Wort „Fehler“ statt „Gestoppt“ gekennzeichnet.

Die Schaltflächen einer Zeile werden nur für die vom Host übergebenen Funktionen erzeugt und sind nur dort aktiviert, wo der Zustand es zulässt:

- Start — nur bei einer Strategie im Zustand `Stopped`;
- Stopp — nur bei einer Strategie im Zustand `Started`;
- Position schließen — nur bei einer laufenden Strategie mit von null verschiedener Position;
- Risikoregeln und Wechsel zur Strategie — immer.

Die Auswahlliste des Handelsmodus ist nur bei einer gestoppten Strategie aktiv: Der Modus bestimmt, womit die Strategie gestartet wird, und dient nicht als Hebel während des Handels. Eine Änderung des Modus ruft `setTradingMode` auf; das Steuerelement ändert den Wert in der Zeile nicht selbst und wartet auf das nächste `update`.

## Spalten und Gestaltung

Die Tabelle zeigt Zustand, Aktionen, Online-Kennzeichen, Handelsmodus, Name, Portfolio, Instrument, Position, Anzahl der Aufträge und Trades, Gewinnänderung, Gewinndiagramm, realisierten und nicht realisierten Gewinn sowie Fehler. Das Online-Kennzeichen ist gemeinsam: Eine Strategie gilt nur dann als online, wenn sie sowohl gebildet als auch verbunden ist, und darüber entscheidet der Datenlieferant.

Die Farbklassen für die Position, die Gewinnänderung und beide Gewinngrößen liefert `host.presentation.pnlClass`. Die Gewinnänderung wird zusätzlich mit einem Richtungspfeil versehen; bei einer Änderung von null gibt es keinen Pfeil.

Die Diagrammspalte zeichnet die Kurve des kumulierten Gewinns anhand der Punkte `pnl` in einem Feld von 140 × 26 CSS-Pixeln. Das Canvas wird unter Berücksichtigung von `devicePixelRatio` erzeugt, deshalb bleibt die Linie auf Bildschirmen mit hoher Dichte scharf. Die Farben stammen aus `host.presentation.canvasPalette()`, und die Kurve wird nach dem Ergebnis des Laufs eingefärbt: Eine Strategie, die ihr Hoch erreicht und alles wieder abgegeben hat, wird als verlustbringend dargestellt. Ohne `pnl`-Punkte bleibt die Zelle leer.

Die Standardsortierung ist aufsteigend nach Name: Die Liste wird von oben nach unten auf der Suche nach einer bestimmten Strategie gelesen, und ein Umsortieren der Zeilen entlang des Gewinns stört diese Lesart. Das Panel unterstützt außerdem Mehrfachauswahl von Zeilen, ein Kontextmenü, Filter und den Export nach XLSX.

## Was dem Host bleibt

Das Steuerelement startet und stoppt keine Strategien, sendet keine Aufträge und schließt keine Positionen — es ruft die übergebenen Funktionen auf und wartet auf eine neue Zeilenliste.

Den Wert `pnlChange` übernimmt das Steuerelement, wie er ist: Den Bezugspunkt wählt der Datenlieferant. So rechnet eine neu gestartete Strategie die Änderung nicht ab einem Zeitpunkt vor dem Neustart weiter.

Die Schaltfläche zum Schließen des Panels ruft `host.close()` auf, der Export gibt die Datei `strategies` aus. Die Instanz registriert sich beim Erstellen im Host und meldet sich in `dispose` wieder ab.

## Öffentliche Methoden

- `StrategiesWidget.create(hostEl, state, deps)` — das Panel aufbauen und in den Container einfügen.
- `update(rows)` — die gesamte Strategieliste ersetzen.
- `dispose()` — die Registrierung aufheben, die Tabelle entfernen und Ressourcen freigeben.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Positionen](positions.md)
- [Aktive Aufträge](active_orders.md)
- [Ausführungshistorie](trade_history.md)
