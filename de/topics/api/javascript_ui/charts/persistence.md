# Layout speichern

`ChartStatePersistence` sammelt das Layout eines Charts — Optionen, Panels, Kursskalen, Serien, Indikatoren und grafische Objekte — in einer einzigen geprüften JSON-Momentaufnahme und stellt es daraus wieder her. Die Balkendaten gelangen nicht in die Momentaufnahme: Gespeichert wird die Konfiguration, die Quotierungen kommen aus Ihrer Quelle.

Die Schicht besitzt weder den Speicher noch die Regel für die Schlüsselbenennung. Wohin geschrieben wird (Datei, Backend, `localStorage`, IndexedDB) und wie die Momentaufnahmen getrennt werden (nach Layout, nach Instrument, nach Benutzer), entscheidet die Anwendung — über eine Implementierung von `ChartStateStorage` und die Funktion `key`.

## Erstellen und aktualisieren

Der Import erfolgt über den Einstiegspunkt `@stocksharp/chart/persistence`:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { DrawingController } from '@stocksharp/chart/drawings';
import {
  ChartStatePersistence,
  NativeChartLayoutAdapter,
  IndicatorEngineStateAdapter,
  type ChartStateStorage,
  type IndicatorEnginePersistenceApi,
} from '@stocksharp/chart/persistence';

declare const indicatorEngine: IndicatorEnginePersistenceApi;

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {});
chart.addSeries(CandlestickSeries, { id: 'price', upColor: '#26a69a', downColor: '#ef5350' });

const storage: ChartStateStorage = {
  load: key => localStorage.getItem(key),
  save: (key, value) => { localStorage.setItem(key, value); },
  remove: key => { localStorage.removeItem(key); },
};

const persistence = new ChartStatePersistence<{ layoutId: string; symbol: string }>({
  layout: new NativeChartLayoutAdapter({ chart, mainPaneId: 'main' }),
  indicators: new IndicatorEngineStateAdapter({ engine: indicatorEngine }),
  drawings: new DrawingController({ chart }),
  storage,
  key: ({ layoutId, symbol }) => `chart:${layoutId}:${symbol}`,
  pretty: true,
});

const context = { layoutId: 'desk', symbol: 'BTC@IMEX' };

await persistence.save(context);

const restored = await persistence.load(context);
if (restored !== null) {
  console.log(restored.state.panes.length);
  console.log(restored.drawings.skipped);   // Objekte unbekannter Typen wurden übersprungen, nicht die Wiederherstellung verloren
}
```

Der Typparameter `TContext` ist das, was Sie an `save`, `load` und `remove` übergeben; die Funktion `key` verwandelt den Kontext in eine Schlüsselzeichenfolge und muss eine nicht leere Zeichenfolge zurückgeben. `pretty: true` schreibt das JSON mit Einrückungen.

`DrawingController` muss dieselbe Instanz sein, die das Zeichnen auf dem Chart bedient — andernfalls wird ein leerer Objektsatz gespeichert.

## Adapter

`ChartStatePersistence` weiß weder von der nativen API des Charts noch von der Indikator-Engine: Sie arbeitet über zwei Adapter. Fertige Implementierungen gehören zum selben Modul, doch bei einer eigenen Architektur lassen sich eigene übergeben — die Schnittstellen `ChartStateLayoutAdapter` (`capture`, `restore`) und `ChartStateIndicatorAdapter` (`capture`, `clear`, `restore`) sind offen.

**`NativeChartLayoutAdapter`** nimmt das Layout des Charts selbst auf und stellt es wieder her: die Chart-Optionen, die Panels mit ihrer Reihenfolge, Höhe, Mindesthöhe und ihrem Zustand (`normal`, `minimized`, `maximized`), die Einstellungen der Kursskalen sowie die Serien mit ihrem Typ, Panel, ihrer Skala und ihren Stiloptionen. Die Optionen des Konstruktors:

- `chart` — die Chart-Instanz (erforderlich).
- `mainPaneId` — die Kennung des Wurzelpanels, das die Wiederherstellung überdauert; standardmäßig `main`, sonst das erste Panel.
- `createSeries(series, pane)` — eigenes Erzeugen einer Serie anstelle des Typregisters, wenn die Serie an eine Datenquelle angebunden werden muss.
- `includeSeries(series)` — Filter: Eine Serie, für die `false` zurückgegeben wurde, wird nicht gespeichert und bei der Wiederherstellung nicht entfernt.
- `onRemoveSeries(series)` — wird für eine fremde Serie aufgerufen, die die Wiederherstellung dennoch abhängen musste, weil ihr Panel nicht zum geladenen Layout gehört.
- `onUnknownSeries(series)` — der Serientyp fehlt im Register.

Eine Serie mit der Option `persist: false` wird aus der Momentaufnahme ebenso ausgeschlossen wie eine vom Filter `includeSeries` abgelehnte.

**`IndicatorEngineStateAdapter`** speichert die Konfiguration der Indikatoren — Typ, Parameter, Zeichenstile, Bindung an Panel und Skala, Sichtbarkeit und Quelle —, jedoch nicht die berechneten Werte: Nach der Wiederherstellung werden sie neu berechnet. Die Optionen des Konstruktors:

- `engine` — die Indikator-Engine, die `IndicatorEnginePersistenceApi` implementiert (`getIndicators`, `removeAll`, `add`, `setVisible`).
- `resolveTargetPaneId(indicator)` — Zuordnung des gespeicherten Panels zum Panel des Hosts, wenn sich die Kennungen unterscheiden.
- `onUnknownIndicator(indicator)` — die Engine konnte einen Indikator dieses Typs nicht erzeugen.
- `onUnknownStyle(indicator, styleId)` — in den Stilen ist eine Kennung aufgetaucht, die der Indikator nicht hat.

Ein Indikator, der über die Ausgabe eines anderen Indikators berechnet wird, wird nach diesem wiederhergestellt: Der Adapter ordnet die Kette der Quellen selbst und meldet einen Fehler, wenn ein Verweis auf einen fehlenden Indikator führt oder der Graph zyklisch ist.

## Zustandsformat und Migrationen

Die Momentaufnahme wird durch den Typ `ChartStateV1` mit den Feldern `schemaVersion`, `chartOptions`, `panes`, `series`, `indicators`, `drawings` beschrieben; die aktuelle Schemaversion ist die Konstante `CHART_STATE_SCHEMA_VERSION` (gleich 1).

- `serializeChartState(state, { pretty })` — den Zustand prüfen und in eine JSON-Zeichenfolge verwandeln.
- `deserializeChartState(value, { migrations })` — die Zeichenfolge zerlegen (oder ein bereits fertiges Objekt entgegennehmen), die Migrationen bis zur aktuellen Version durchlaufen und das Ergebnis prüfen.
- `normalizeChartStateV1(value)` — Prüfung und Einfrieren des Zustands: fremde Schlüssel, doppelte Kennungen, Verweise auf nicht vorhandene Panels und ein Layout ohne ein einziges Panel werden abgelehnt.
- `normalizePersistedObject(value, path, { omitUndefined })` — tiefes Kopieren eines beliebigen JSON in ein unveränderliches Objekt; Zyklen, nicht numerische Werte, übermäßige Verschachtelung und die Schlüssel `__proto__`, `prototype`, `constructor` sind verboten.

Alte Momentaufnahmen werden durch schrittweise Migrationen angehoben. Das gemeinsame Register `chartStateMigrations` enthält bereits den Übergang von Version 0 auf Version 1, und eigene Schritte werden so registriert:

```ts
import {
  ChartStateMigrationRegistry,
  deserializeChartState,
} from '@stocksharp/chart/persistence';

const migrations = new ChartStateMigrationRegistry();
migrations.register(1, state => ({ ...state, schemaVersion: 2 }));

const state = deserializeChartState(json, { migrations });
```

Jede Migration bringt den Zustand genau eine Version weiter und muss den neuen Wert von `schemaVersion` setzen. Eine Momentaufnahme, deren Version über der unterstützten liegt, wird nicht zum Laden angenommen.

## Öffentliche Methoden

- `snapshot()` — den aktuellen Chart-Zustand als `ChartStateV1` sammeln, ohne den Speicher anzusprechen.
- `restore(state)` — den Zustand auf das Chart anwenden; gibt `{ state, drawings }` zurück, wobei `drawings` die Listen `restored` und `skipped` enthält.
- `save(context)` — eine Momentaufnahme aufnehmen, serialisieren und unter dem berechneten Schlüssel in den Speicher schreiben; gibt den gespeicherten Zustand zurück.
- `load(context)` — den Eintrag zum Schlüssel lesen, die Migrationen durchlaufen und ihn wiederherstellen; `null`, wenn es keinen Eintrag gibt.
- `remove(context)` — den Eintrag aus dem Speicher löschen.

Die Reihenfolge der Wiederherstellung liegt fest: zuerst werden die Indikatoren freigegeben, dann das Layout der Panels und Serien wiederhergestellt, dann die Indikatoren und zuletzt die grafischen Objekte.

Das Modul exportiert außerdem die Typen zur Beschreibung der Momentaufnahme und der Adapter: `ChartStateLayoutSnapshot`, `ChartStateRestoreResult`, `ChartStatePersistenceOptions`, `PersistedPane`, `PersistedPriceScale`, `PersistedSeries`, `PersistedIndicator`, `PersistedDrawing`, `PersistedChartOptions`, `PersistedSeriesOptions`, `PersistedIndicatorParameters`, `PersistedIndicatorStyles`, `PersistedObject`, `PersistedJsonValue`, `PersistableIndicatorEntry`, `RawChartState`, `ChartStateMigration`, `MaybePromise`.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Indikatoren](indicators.md)
- [Nachladen der Historie](backfill.md)
