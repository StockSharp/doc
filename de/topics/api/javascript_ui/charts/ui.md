# Chart-Oberfläche

`createChartUi` baut rund um die Engine eine fertige Oberfläche auf: Panel-Titelleisten, eine Legende mit Fadenkreuz, ein Kontextmenü, ein Menü für den Chart-Typ und einen Indikator-Dialog. Die Engine zeichnet nur; alles rundherum lebt im eigenen Einstiegspunkt `@stocksharp/chart/ui` — eine Seite mit einer einzelnen Sparkline zahlt dafür nicht mit.

## Einbinden

Die Schicht wird als eigener Unterpfad des Pakets ausgeliefert und benötigt ihr eigenes Stylesheet:

```ts
import { createChartUi, standaloneHost } from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';
```

Ohne Bundler binden Sie das Browserpaket `dist/sschartui.js` ein — es stellt das globale Objekt `SSChartUI` bereit. Es muss **nach** `dist/sschart.js` geladen werden: Die Schicht liest die Engine aus dem von dieser Datei bereitgestellten globalen Objekt und bringt keine zweite Kopie davon mit.

## Erstellen und aktualisieren

Die Schicht benötigt das Element, in dem das Chart erstellt wurde, den Host der Seite, eine Kursquelle für das Kontextmenü und die Liste der Chart-Typen für das Legendenmenü:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  ChartType,
  createChartUi,
  localChartUiStorage,
  standaloneHost,
  type ChartUi,
} from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';

declare const candles: { time: number; open: number; high: number; low: number; close: number; volume?: number }[];

const container = document.querySelector<HTMLElement>('#chart')!;

const chart = createChart(container, { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
series.setData(candles);

const ui: ChartUi = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [
    { value: ChartType.Candle, label: 'Candles', icon: 'bi bi-bar-chart-fill' },
    { value: ChartType.Line, label: 'Line', icon: 'bi bi-graph-up' },
  ],
  storage: localChartUiStorage('sschart'),
});

ui.setCandles(candles);
```

`setCandles` wird bei jedem Wechsel des Kerzenfensters erneut aufgerufen — neues Instrument, umgeschalteter Chart-Typ, eingetroffene Historienseite. Ein Aufruf aktualisiert sowohl die Indikator-Engine als auch die Legende.

Die Optionen von `createChartUi`:

| Option | Bedeutung |
|---|---|
| `container` | Das Element, in dem das Chart erstellt wurde; rund um dieses werden die Panels aufgebaut. |
| `host` | Übersetzung, Formatierung und Meldungen. |
| `priceSource` | Pixel → Kurs für das Kontextmenü. |
| `chartTypes` | Die Einträge des Chart-Typ-Menüs in der Anzeigereihenfolge; eine leere Liste zeichnet kein Menü. |
| `storage` | Wo bevorzugte Indikatoren und Vorlagen gespeichert werden. Standardmäßig im Speicher. |
| `dialogRoot` | Eigenes Markup des Indikator-Dialogs. Ohne es wird das Markup aufgebaut und in `body` eingefügt. |
| `modal` | Eine eigene Implementierung für das Öffnen und Schließen des Dialogs. |
| `provideItems` | Die Einträge der Seite im Kontextmenü — über den Einträgen der Schicht selbst. |

## Was createChartUi zurückgibt

Das Ergebnis sind dieselben, bereits miteinander verbundenen Objekte; an jedes davon kommt man direkt heran.

| Feld | Was es ist |
|---|---|
| `engine` | `IndicatorEngine` — Berechnung und Lebenszyklus der Indikatoren. |
| `renderer` | `IndicatorRenderer` — die Serien, mit denen die Ausgaben der Indikatoren gezeichnet werden. |
| `paneManager` | `ChartPaneManager` — Panel-Titelleisten, ihre Menüs und ihre Wiederherstellung. |
| `legend` | `ChartLegend` — die OHLCV-Zeile und die Indikatorwerte unter dem Fadenkreuz. |
| `dialog` | `IndicatorDialog` — Katalog, Suche, Parameter und aktive Indikatoren. |
| `menu` | `ChartContextMenu` — das Menü der rechten Maustaste. |
| `indicators` | `IndicatorController` — rückgängig machbares Bearbeiten bereits hinzugefügter Indikatoren. |
| `templates` | `IndicatorTemplateController` — übertragbare Vorlagen für Indikatoreinstellungen. |

## Öffentliche Methoden

- `setCandles(candles)` — das aktuelle Kerzenfenster an die Indikator-Engine und die Legende übergeben.
- `showIndicators()` — den Indikator-Dialog öffnen.
- `dispose()` — Menü, Dialog, Legende und Panels entfernen; das von der Schicht erzeugte Dialog-Markup wird gelöscht.

## Host der Seite

Kein Modul der Schicht greift auf globale Objekte zu: Wörter, Zahlen und Meldungen kommen aus dem `ChartUiHost` — einem Objekt mit den Feldern `translate`, `formatters` und `notify`.

```ts
import { consoleNotify, createTranslate, defaultChartFormatters, type ChartUiHost } from '@stocksharp/chart/ui';

const host: ChartUiHost = {
  translate: createTranslate({ 'Indicators': 'Indikatoren', 'Add indicator…': 'Indikator hinzufügen…' }),
  formatters: { ...defaultChartFormatters, price: value => value.toFixed(2) },
  notify: consoleNotify,
};
```

Das Wörterbuch ist flach und über den englischen Ausgangstext verschlüsselt: Ein nicht beantworteter Schlüssel gibt sich selbst zurück, also eine lesbare englische Zeichenfolge und keine Markierung für eine fehlende Übersetzung. Die Einsetzung erfolgt über Positionen — `{0}`, `{1}`.

- `standaloneHost` — ein Host, der alles selbst übernimmt: englischer Text, Formatierung nach Größe der Zahl, Meldungen in der Konsole.
- `identityTranslate` — die Übersetzung für eine einsprachige Seite; die Einsetzung der Platzhalter bleibt erhalten.
- `defaultChartFormatters` — `price`, `volume` und `time` (Unix-Sekunden, Format `YYYY-MM-DD HH:MM`).
- `consoleNotify` — Ausgabe der Meldungen in der Browserkonsole; die Stufen `success`, `info`, `warning`, `error`.
- `createPlainModalController(root)` — Öffnen und Schließen des Dialogs für eine Seite ohne eigene Bibliothek für Modalfenster: Hintergrundebene und Schließen mit `Escape`. Ein Klick neben das Fenster schließt den Dialog nicht.

## Speicher

Bevorzugte Indikatoren und Vorlagen werden über `ChartUiStorage` gespeichert — zwei Funktionen, `load(key)` und `save(key, value)`.

- `inMemoryChartUiStorage` — der Standardwert: Die Daten leben so lange wie die Seite.
- `localChartUiStorage(prefix)` — eine Hülle um `localStorage` mit einem Präfix, damit zwei Charts auf einer Seite nicht gegenseitig ihre Favoriten überschreiben.

## Chart-Typ

`ChartTypeSwitcher` zeichnet dasselbe Balkenfenster als Kerzen, Balken, Linie, Fläche, Heikin-Ashi, Renko oder Point & Figure neu. Ein Typwechsel bedeutet einen anderen Renderer, deshalb wird die Serie neu erzeugt und ihre bisherige Instanz ungültig:

```ts
import {
  ChartType,
  ChartTypeSwitcher,
  allChartTypes,
  defaultChartTypePalette,
  parseChartType,
} from '@stocksharp/chart/ui';

const switcher = new ChartTypeSwitcher({
  chart,
  series,
  initialType: ChartType.Candle,
  availableTypes: allChartTypes,
  palette: defaultChartTypePalette,
  host: standaloneHost,
});
switcher.setRawCandles(candles);

switcher.onSeriesChanged(next => ui.menu.setPriceSource(next));

ui.legend.onChartTypeChange = value => {
  const type = parseChartType(value);
  if (type === null) return;

  switcher.switchType(type);
  ui.setCandles(switcher.getIndicatorCandles());
};
```

`parseChartType` liest den Typ aus einer Zeichenfolge, die die Seite selbst gespeichert hat — aus einem gespeicherten Layout oder einem Schaltflächenattribut —, und gibt `null` zurück, wenn es einen solchen Typ nicht gibt. `getIndicatorCandles` liefert die Balken, über die nach dem Umschalten die Indikatoren berechnet werden: Renko und Point & Figure bauen die Ausgangsbalken zu eigenen um, und `isDerivedChartType(type)` beantwortet, ob das geschehen ist — solche Balken haben auch kein Volumen. Die übrigen Methoden: `getCurrentSeries`, `getCurrentType`, `getAvailableTypes`, `updatePrice`.

## Kontextmenü

`ChartContextMenu` fügt keine eigenen Einträge hinzu — sie kommen von der Seite über `provideItems`, das Gruppen von Einträgen zurückgibt; zwischen den Gruppen wird ein Trenner gezeichnet, leere Gruppen kosten nichts. Ein Eintrag wird durch den Schlüssel `key`, den Text `label`, die optionalen `icon`, `tone` und `disabled` sowie die Methode `invoke` beschrieben. Den Ton legen die Werte von `ChartContextMenuTone` fest: `Neutral`, `Positive`, `Negative`.

```ts
import { ChartContextMenuTone, createChartUi, standaloneHost } from '@stocksharp/chart/ui';

const ui = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [],
  provideItems: context => [[
    {
      key: 'buy',
      label: `Kaufen zu ${context.priceText}`,
      icon: 'bi bi-arrow-up-circle',
      tone: ChartContextMenuTone.Positive,
      invoke: () => placeOrder('buy', context.price),
    },
  ]],
});
```

Zu diesen Einträgen fügt `createChartUi` seine eigene Gruppe hinzu — `Add indicator…` und `Add pane…`, beide über `host.translate`. `ChartContextMenuMode` unterscheidet das Menü auf dem Kurs-Chart (`Chart`, es gibt einen Kurs unter dem Cursor) von dem auf der Titelleiste eines Unter-Panels (`Pane`, ohne Kurs). Die Methoden: `init`, `setPriceSource`, `openAt`, `close`, `dispose`.

## Die übrigen Exporte

- `ChartLegend` und `fullscreenMenuLayer` — die Legende und die Ebene, in der sich ihr schwebendes Menü öffnet (das Vollbildelement, falls vorhanden, sonst `body`). Die Methoden der Legende: `init`, `setRawCandles`, `setChartType`, `setIndicatorEngine`, `refresh`, `dispose`; die Rückrufe `onEditIndicator` und `onChartTypeChange`.
- `ChartPaneManager` — die Hülle um die eingebauten Panels der Engine: `init`, `addPane`, `removePane`, `restorePane`, `getChart`, `getPanes`, `getPaneByMeasure`, `setPaneTitle`, `getValuesElement`, `legendLayer`, `resize`, `dispose`.
- `IndicatorDialog` und `createIndicatorCatalogController` — der Indikator-Dialog und das Modell seines Katalogs. Die Methoden des Dialogs: `show`, `showForPane`, `showEdit`, `hide`, `dispose`.
- `IndicatorEngine`, `IndicatorRenderer`, `IndicatorSettings` — die Mechanik der Indikatoren, die der Dialog steuert. Wird sowohl von hier als auch aus `@stocksharp/chart/indicators` veröffentlicht.
- Die Typen `LegendBar`, `LegendChartType`, `LegendChart`, `LegendPaneHost`, `LegendIndicatorEngine`, `IndicatorPaneChart`, `IndicatorPaneHost`, `ChartContextMenuProvider`, `PriceCoordinateSource`, `ChartTypePalette`, `ModalController` — strukturelle Verträge: Eine Seite, die die Panels selbst anordnet oder die Indikatoren selbst berechnet, implementiert sie mit eigenen Objekten.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Indikatoren](indicators.md)
- [Nachladen der Historie](backfill.md)
- [Kerzenchart](candlestick.md)
