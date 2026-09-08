# Statistik

`StatisticsWidget` ist eine Tabelle der Statistikparameter einer Strategie: Gewinn, Drawdown, Anzahl der Trades, Latenzen. Das Panel ist ausschließlich zum Lesen gedacht: eine Zeile je Parameter, wobei die Zeilen nach dem Bereich gruppiert sind, zu dem der jeweilige Parameter gehört.

![Statistikpanel eines Laufs mit gruppierten Kennzahlen](../../../../images/javascript_controls_statistics.png)

## Erstellen und aktualisieren

```ts
import {
  StatisticsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const statistics = StatisticsWidget.create(
  document.querySelector<HTMLElement>('#statistics')!,
  {},
  { host },
);

statistics.update([
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: 'Gewinn',
    order: 1,
    name: 'Nettogewinn',
    description: 'Ergebnis des Laufs',
    value: 11_055.75,
  },
  {
    key: 'MaxProfitDate',
    category: 'pnl',
    categoryText: 'Gewinn',
    order: 2,
    name: 'Datum des Maximums',
    value: '2024-03-26T07:30:00Z',
  },
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: 'Trades',
    order: 100,
    name: 'Anzahl der Trades',
    value: 1_340,
  },
]);
```

Der Abhängigkeitssatz `StatisticsDeps` besteht aus dem einzigen Pflichtfeld `host`. Aktionshandler hat das Steuerelement nicht: Die Statistik erzeugt die Strategie, und im Panel gibt es nichts abzubrechen, neu zu laden oder zu bearbeiten. Das zweite Argument von `create` ist der gespeicherte Panelzustand; das Steuerelement verwendet ihn nicht.

`update` ersetzt den gesamten Zeilensatz vollständig. Die Strategie veröffentlicht ihre Parameter als eine einzige Tabelle, deshalb gilt eine aus dem Satz verschwundene Zeile als nicht mehr existent und nicht als unverändert. Die Identität einer Zeile bestimmt das Feld `key`.

## Zeilen und Reihenfolge

Eine Zeile wird durch den Typ `StatisticRow` beschrieben:

| Feld | Bedeutung |
|---|---|
| `key` | Stabile Kennung des Parameters. |
| `category` | Sprachunabhängiger Schlüssel der Gruppe. |
| `categoryText` | Anzuzeigende Überschrift der Gruppe. Fehlt sie, wird `category` verwendet. |
| `order` | Platz des Parameters im Verzeichnis. |
| `name` | Lokalisierte Bezeichnung des Parameters. |
| `description` | Lokalisierte Erläuterung, wird als Tooltip auf der Namenszelle angezeigt. |
| `value` | Zahl, Datum oder Zeichenfolge. `null` — der Parameter wurde noch nicht gemessen. |

Die Desktop-Variante der Tabelle bezieht die Zeilen per Reflexion über die Parameter der Strategie; im Browser gibt es einen solchen Mechanismus nicht, deshalb kommen die Zeilen fertig vom Host — mit übersetzter Bezeichnung und Erläuterung.

Gruppiert wird nach `category` und nicht nach `categoryText`: Eine Gruppierung nach der übersetzten Überschrift würde die Tabelle bei einem Sprachwechsel umbauen. Die Reihenfolge der Gruppen ergibt sich aus dem kleinsten `order` unter ihren Parametern, deshalb steht der Gewinn über dem Drawdown und dieser über den Trade-Zählern. Eine Sortierung nach Name oder Wert würde Parameter auseinanderreißen, die zusammen gelesen werden.

Sichtbar sind zwei Spalten: `Name` und `Value`. Die Spalten `category` und `order` sind deklariert, aber ausgeblendet — sie werden für Gruppierung und Sortierung benötigt und sagen dem Leser nichts. In den Export gelangen nur die beiden sichtbaren Spalten.

## Formatierung der Werte

Den Text der Wertzelle erzeugt die exportierte Funktion `formatStatistic(value)`:

```ts
import { formatStatistic } from '@stocksharp/trading-controls';

formatStatistic(11_055.756); // '11055.76'
formatStatistic(1_340);      // '1340'
formatStatistic('2024-03-26T07:30:00Z'); // '2024-03-26'
formatStatistic(null);       // ''
```

Eine Zahl wird auf zwei Stellen gerundet und ohne nachlaufende Nullen angezeigt. Ein Datum erscheint nur als Tag: Parameter dieser Art beschreiben den gesamten Lauf, und die Uhrzeit wäre darin nur Rauschen. Eine Zeichenfolge wird nur dann als Datum erkannt, wenn sie mit `JJJJ-MM-TT` beginnt; andernfalls bleibt sie Text. Ein fehlender Wert ergibt eine leere Zelle und keine Null, die als gemessenes Ergebnis gelesen würde.

Sortiert wird nach dem ursprünglichen Wert, deshalb wird eine Zahl als Zahl und nicht als Zeichenfolge sortiert.

## Was das Steuerelement tut und was der Host

Das Steuerelement bezieht den gesamten sichtbaren Text über `host.t` vom Host, einschließlich der Spaltenbeschriftungen, der Panelüberschrift, des Hinweises für die leere Tabelle und der Einträge des Tabellenkontextmenüs. Die Schaltfläche zum Schließen ruft `host.close` auf, der Export gibt die Tabelle als XLSX aus. Beim Erstellen registriert sich die Instanz über `host.register`, bei `dispose` meldet sie sich über `host.unregister` ab.

Eigene Einstellungen speichert das Steuerelement nicht in `host.preferences`. Daten fordert es nicht an: Die Zeilen liefert der Host mit dem Aufruf `update`.

Die Kennung des Paneltyps ist als `StatisticsWidget.TYPE` verfügbar und entspricht `ControlTypes.Statistics`.

## Öffentliche Methoden

- `StatisticsWidget.create(hostEl, state, deps)` — das Panel im angegebenen Container erstellen.
- `update(rows)` — den gesamten Satz der Statistikzeilen ersetzen.
- `dispose()` — Ressourcen freigeben.

Das Panel unterstützt außerdem Sortierung, Mehrfachauswahl von Zeilen, ein Kontextmenü und den Export nach XLSX.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Positionen](positions.md)
- [Ausführungshistorie](trade_history.md)
- [Aktive Aufträge](active_orders.md)
