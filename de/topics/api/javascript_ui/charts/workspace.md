# Mehrere Charts

`MultiChartWorkspace` ordnet mehrere unabhängige Charts als Raster in einem Container an, verknüpft sie über Instrument und Zeitrahmen und synchronisiert den sichtbaren Bereich und das Fadenkreuz. Die Klasse wird über den Einstiegspunkt `@stocksharp/chart/workspace` ausgeliefert, zusammen mit den übrigen Controllern des Arbeitsbereichs — für Panels, Indikatoren, Vorlagen, Instrumentenvergleich und Historiennavigation.

Der Arbeitsbereich besitzt nur die Charts der obersten Ebene. Indikator-Panels bleiben eine interne Angelegenheit ihres Charts: Sie gelten nicht als Zellen, nehmen nicht am Layout teil und werden nicht synchronisiert.

## Erstellen und aktualisieren

Container und Chart-Factory sind erforderlich. Die Factory erhält `{ id, index, host }` und gibt eine Zelle zurück — das Chart selbst, einen optionalen Datencontroller und eine optionale Freigabefunktion (standardmäßig wird `chart.remove()` aufgerufen):

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { ChartDataController, type IChartDataSource } from '@stocksharp/chart/data';
import { MultiChartWorkspace } from '@stocksharp/chart/workspace';

declare const dataSource: IChartDataSource;

const workspace = new MultiChartWorkspace({
  container: document.querySelector<HTMLElement>('#workspace')!,
  count: 4,
  columns: 2,                                   // null — automatisches, nahezu quadratisches Raster
  links: { symbol: true, resolution: false },   // gemeinsames Instrument, eigener Zeitrahmen je Zelle
  sync: { range: true, crosshair: true },
  createChart: ({ host }) => {
    const chart = createChart(host, { timeScale: { timeVisible: true } });
    const series = chart.addSeries(CandlestickSeries, {
      upColor: '#26a69a',
      downColor: '#ef5350',
    });
    const data = new ChartDataController({ chart, series, dataSource, initialCount: 300 });

    return {
      chart,
      data,
      dispose: () => {
        data.dispose();
        chart.remove();
      },
    };
  },
});

const [first] = workspace.cells();
await workspace.setSelection(first.id, { symbol: 'BTC@IMEX', resolution: '1h' });

workspace.subscribe(snapshot => {
  console.log(snapshot.activeId, snapshot.columns, snapshot.rows, snapshot.errors.length);
});
```

`setCount` und `setColumns` ändern die Rastergröße einzeln, `setLayout({ count, columns })` in einem Vorgang. Der Container wird als CSS-Raster gestaltet; die ursprünglichen Stile werden gemerkt und in `dispose` wiederhergestellt. Es kann 1 bis 64 Zellen geben; die letzte Zelle lässt sich nicht entfernen.

## Verknüpfung und Synchronisation

`links` beschreibt, was bei einem Instrumentenwechsel auf die übrigen Zellen übertragen wird: `symbol` und `resolution` lassen sich unabhängig voneinander aktivieren. Eine Zelle, für die die Factory kein `data` zurückgegeben hat, nimmt an der Verknüpfung nicht teil.

`sync` aktiviert die Übertragung des sichtbaren Bereichs (`range`) und der Position des Fadenkreuzes (`crosshair`). Der Bereich wird nach dem Anwenden vom empfangenden Chart erneut ausgelesen: Eine Zelle mit kürzerer Historie beschneidet das angeforderte Fenster, und in der Momentaufnahme wird veröffentlicht, was tatsächlich angezeigt wird.

Die aktive Zelle wird mit `activate` gesetzt sowie automatisch bei `pointerdown` und `focusin` innerhalb einer Zelle. Ein Wechsel von `links` oder `sync` verteilt den aktuellen Zustand der aktiven Zelle sofort an die übrigen.

Synchronisationsfehler unterbrechen die Arbeit der übrigen Zellen nicht, sondern sammeln sich in `snapshot.errors` — die letzten 32. Jeder Eintrag hat `cellId`, `kind` (`WorkspaceSyncErrorKind`: `selection`, `range`, `crosshair`, `lifecycle`) und `error`. Die Liste wird mit dem Aufruf `clearErrors` geleert.

## Öffentliche Methoden

- `snapshot()` — der vollständige Zustand: Anzahl der Zellen, Spalten und Zeilen, aktive Zelle, `links`, `sync`, Zellen und Fehler.
- `cells()` — Momentaufnahmen der Zellen: `id`, `index`, `active`, `selection`, `visibleRange`, `crosshairTime`.
- `chart(id)` / `host(id)` — das Chart und das DOM-Element einer Zelle.
- `add(id?)` — eine Zelle hinzufügen; ohne Argument wird die Kennung erzeugt.
- `remove(id)` — eine Zelle entfernen.
- `setCount(count)`, `setColumns(columns)`, `setLayout(layout)` — das Raster ändern.
- `activate(id)` — eine Zelle aktiv setzen.
- `setLinks(options)`, `setSync(options)` — Verknüpfung und Synchronisation umschalten.
- `setSelection(id, selection)` — Instrument und Zeitrahmen einer Zelle setzen und über die Verknüpfungen verteilen.
- `clearErrors()` — die gesammelten Fehler löschen.
- `subscribe(listener)` / `unsubscribe(listener)` — Abonnement auf die Zustands-Momentaufnahme.
- `dispose()` — die Zellen freigeben und die Container-Stile wiederherstellen.

## Die übrigen Controller der Schicht

- `PaneController` — rückgängig machbares (undo/redo) Verwalten der Chart-Panels: `resizePair`, `reorder`, `moveSeries`, `setState`, `toggleMinimized`, `toggleMaximized`. Arbeitet über den gemeinsamen Befehlsstapel des Charts und erzeugt den Inhalt der Panels nicht neu.
- `IndicatorController` — geprüftes Bearbeiten der Indikatoren über der Rechen-Engine: `update`, `setParameters`, `setSource`, `moveToPane`, `setPriceScale`, `setVisible`, `setOutputStyle`. Jede Änderung gelangt in den Befehlsstapel, die Momentaufnahme enthält die Parameterdefinitionen, den Zustand der Quelle und die Stile der Ausgaben.
- `IndicatorCatalogController` — Suche im Indikatorkatalog (`search` nach Text, Kategorie und Favoritenkennzeichen) und die vom Host gespeicherten Favoriten: `loadFavorites`, `setFavorite`, `toggleFavorite`.
- `IndicatorTemplateController` — übertragbare Vorlagen für Indikatoreinstellungen: `create`, `replace`, `rename`, `remove`, `apply`, `load`. Die Methode `apply` überträgt Parameter, Quelle, Sichtbarkeit und Stile der Ausgaben, lässt Panel und Kursskala des Ziels jedoch bewusst unverändert.
- `serializeIndicatorTemplates`, `deserializeIndicatorTemplates`, `normalizeIndicatorTemplateDocument`, `INDICATOR_TEMPLATE_SCHEMA_VERSION` — Serialisierung und Prüfung des versionierten Vorlagendokuments.
- `CompareController` — Überlagerung mehrerer Instrumente auf einem Chart: `add`, `remove`, `setPrimary`, `setColor`, `setVisible`, `reload`, `loadMoreBefore`, `legend`. Den Normalisierungsmodus legt `setMode` fest (`CompareMode.Percentage` oder `CompareMode.IndexedTo100`), die Art der Zeitausrichtung `setAlignment` (`CompareAlignment.Chart` oder `CompareAlignment.PrimarySession`). Jedes Instrument hat seinen eigenen `ChartDataController` und sein eigenes Abonnement.
- `ChartNavigator` — Historiennavigation ohne Bindung an das DOM: `setRange`, `selectPreset`, `goToDate`, `cancel`. Der Controller lädt fehlende Historienseiten selbst nach (standardmäßig höchstens 100 je Vorgang) und veröffentlicht ein Übersichtsmodell aus einer begrenzten Zahl von Stichproben (standardmäßig 600). Die fertigen Presets `1D`, `5D`, `1M`, `3M`, `6M`, `YTD`, `1Y`, `5Y`, `All` liefert `defaultNavigatorPresets`; das Ergebnis eines Vorgangs beschreibt `NavigatorNavigationOutcome` (`applied`, `clamped`, `page-limit`, `empty`, `cancelled`), die Datumsausrichtung `NavigatorDateAlignment`.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Nachladen der Historie](backfill.md)
- [Indikatoren](indicators.md)
