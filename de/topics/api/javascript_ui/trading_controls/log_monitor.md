# Protokoll

`LogMonitorWidget` zeigt das Arbeitsprotokoll an: links ein Baum der Quellen, rechts eine Tabelle der Meldungen der ausgewählten Quelle und ihres gesamten Teilbaums. Eine Meldung wird einmal gespeichert und trägt die Kennung der Quelle, die sie geschrieben hat; die Zahl der gespeicherten Zeilen ist begrenzt.

![Protokoll mit Quellenbaum, Ebenenfilter und Meldungstabelle](../../../../images/javascript_controls_log_monitor.png)

## Erstellen und aktualisieren

```ts
import {
  LogLevels,
  LogMonitorWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const log = LogMonitorWidget.create(
  document.querySelector<HTMLElement>('#log')!,
  {},
  {
    host,
    maxMessages: 20_000,
  },
);

log.setSources([
  { id: 'connector', name: 'Connector' },
  { id: 'strategy-1', name: 'SMA', parentId: 'connector' },
]);

log.append([{
  id: 1,
  time: Date.now(),
  level: LogLevels.Warning,
  sourceId: 'strategy-1',
  message: 'Auftrag abgelehnt: nicht genug Guthaben',
}]);

log.select('connector');
```

Von den Abhängigkeiten ist nur `host` erforderlich: In das Protokoll wird geschrieben, aus ihm heraus wird nicht gehandelt, deshalb benötigt das Steuerelement keine Handler. Die übrigen sind optional:

| Abhängigkeit | Standardwert | Bedeutung |
|---|---|---|
| `maxMessages` | `5000` | Wie viele Meldungen aufbewahrt werden. Überzählige werden vom Listenanfang her verworfen. |
| `chrome` | `true` | Ob eine eigene Titelleiste mit Schließen-Schaltfläche gezeichnet wird. Ein Host, der das Panel selbst beschriftet und schließt (etwa über einen Dock-Reiter), übergibt `false`. |
| `sources` | `true` | Ob der Quellenbaum beim Erstellen angezeigt wird. Das ist nur der Anfangszustand: Der Baum kehrt über das Tabellenkontextmenü oder den Aufruf `showSources` zurück. |

`setSources` übergibt die Liste der Quellen vollständig: Eine verschwundene Quelle fällt aus dem Baum heraus, und die Auswahl wird dabei auf „alle Quellen“ zurückgesetzt. `append` fügt gerade Geschriebenes hinzu, `clear` verwirft alle Meldungen und lässt den Quellenbaum unangetastet.

Die statische Eigenschaft `LogMonitorWidget.TYPE` enthält die Kennung des Steuerelements `logMonitor`.

## Quellen und Filter

Eine Quelle deklariert ihren Elternknoten (`parentId`) und nicht ihre Nachfahren und kann vor dem Elternknoten auftauchen. Der Baum wird aus dem gebaut, was bereits eingetroffen ist: Eine Quelle mit unbekanntem Elternknoten wird zur Wurzel, ein Zyklus wird am ersten Knoten aufgetrennt. Die Zeilen des Baums sind flach, die Verschachtelung wird durch Einrückung dargestellt; die oberste Zeile wählt alle Quellen auf einmal aus.

Gleichzeitig wirken drei Filter: die Menge der aktivierten Ebenen, die Suchzeichenfolge im Meldungstext (ohne Berücksichtigung der Groß-/Kleinschreibung) und der ausgewählte Teilbaum der Quellen. Die Ebenen werden über Schaltflächen der Symbolleiste umgeschaltet — `error`, `warning`, `info`, `debug`, `verbose` aus dem Objekt `LogLevels`; in der schmalen Spalte wird die Ebene als Buchstabe `E`, `W`, `I`, `D`, `V` angezeigt. Die Methode `visible` gibt zurück, was nach allen Filtern übrig bleibt.

Die Tabelle besteht aus den Spalten Quelle, Zeit, Ebene und Meldung, ist aufsteigend nach Zeit sortiert und unterstützt Mehrfachauswahl, Sortierung, das Ausblenden von Spalten, Filter und ein Kontextmenü. Im Menü ist ein Eintrag zum Anzeigen des Quellenbaums ergänzt. Eine Schaltfläche der Symbolleiste gibt die sichtbaren Zeilen als XLSX aus; in den Export gelangen die mit `host.presentation.timeText` formatierte Zeit und der vollständige Name der Ebene statt des Buchstabens.

## Was der Host tut

Das Steuerelement bezieht vom Host die Übersetzungen (`host.t`), das Zeitformat (`host.presentation.timeText`) und die Behandlung des Panelschließens (`host.close`); außerdem registriert es sich über `host.register` und meldet sich in `dispose` wieder ab. Eigene Einstellungen speichert es nicht in `host.preferences`, und den Instanzzustand sichert es nicht: Das zweite Argument von `create` wird der Einheitlichkeit halber entgegengenommen, aber nicht gelesen. Das Sammeln der Meldungen, deren Zustellung und das Wiederherstellen der Panelposition übernimmt der Host.

## Öffentliche Methoden

- `setSources(sources)` — die Liste der Quellen ersetzen.
- `append(messages)` — Meldungen unter Beachtung der Grenze `maxMessages` hinzufügen.
- `clear()` — die Meldungen löschen.
- `select(sourceId)` — den Teilbaum einer Quelle anzeigen; `null` zeigt alle.
- `visible()` — die nach allen Filtern verbliebenen Meldungen.
- `sourcesShown()` — ob der Quellenbaum angezeigt wird.
- `showSources(on)` — den Baum ein- oder ausblenden; der gewählte Quellenfilter bleibt dabei erhalten.
- `dispose()` — Ressourcen freigeben.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Strategien](strategies.md)
- [Statistik](statistics.md)
