# Zeichenwerkzeuge

`DrawingController` ist die Schicht der manuellen Chart-Markierung: Linien, Figuren, Fibonacci-Level und Positionsvorlagen. Der Controller hält die Figuren als reine JSON-Objekte, bindet sie an Canvas-Primitive, führt jede Änderung über den Rückgängig-Stapel des Charts und übernimmt das schrittweise Konstruieren mit der Maus.

## Einbinden

Die Schicht wird als eigener Einstiegspunkt des Pakets `@stocksharp/chart` ausgeliefert:

```ts
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';
```

Der Import des Einstiegspunkts registriert sofort alle eingebauten Markierungstypen im gemeinsamen Katalog `drawingDefinitionRegistry`.

## Erstellen und aktualisieren

Der Controller benötigt nur das Chart; den Befehlsstapel bezieht er standardmäßig ebenfalls von dort (`chart.commandStack()`), deshalb funktionieren Rückgängig und Wiederholen zusammen mit den übrigen Aktionen am Chart:

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';

const chart = createChart(document.getElementById('chart')!, { timeScale: { timeVisible: true } });
chart.addSeries(CandlestickSeries, {}).setData(candles);

const drawings = new DrawingController({ chart });

// Horizontales Level: ein Punkt, die Zeit ist Unix-Zeit in Sekunden.
const level = drawings.create(
  BuiltInDrawingType.HorizontalLine,
  [{ time: 1_712_000_000, price: 68_000 }],
  { options: { color: '#f5c542', lineWidth: 2 } },
);

// Eine Trendlinie über zwei Punkte auf einem Unter-Panel wird über paneId angegeben.
drawings.create(
  BuiltInDrawingType.TrendLine,
  [
    { time: 1_712_000_000, price: 67_400 },
    { time: 1_712_600_000, price: 69_150 },
  ],
  { paneId: 'main' },
);

drawings.updateOptions(level.id, { lineWidth: 3 });
drawings.setLocked(level.id, true);

// Die Momentaufnahme des Satzes kommt nach zOrder sortiert.
drawings.subscribe(items => console.log(items.length));

chart.commandStack().undo();
```

`create` ergänzt die fehlenden Felder: `paneId` standardmäßig `main`, `visible` — `true`, `locked` — `false`, `zOrder` — um eins größer als das aktuelle Maximum, und die Optionen werden über die `defaultOptions` des Typs gelegt. `add` fügt eine fertige Instanz vollständig ein, `duplicate` kopiert eine vorhandene, `remove` und `clear` löschen. Jeder dieser Aufrufe legt genau einen rückgängig machbaren Befehl in die Historie.

`update` ändert eine beliebige Kombination von Feldern (`points`, `options`, `paneId`, `visible`, `locked`, `zOrder`); `updateOptions`, `setVisible`, `setLocked` und `moveToPane` sind Kurzformen für die häufigen Fälle. Vor dem Schreiben wird die Instanz normalisiert: Punkte und Optionen werden auf JSON-Kompatibilität geprüft und eingefroren, die Anzahl der Punkte wird mit dem Schema des Typs abgeglichen und das Panel mit den vorhandenen Panels des Charts.

## Eingebaute Typen

Die Kennungen sind in `BuiltInDrawingType` gesammelt; der Zeichenfolgenwert ist zugleich das Feld `type` der gespeicherten Figur.

| Konstante | Wert | Punkte | Optionen |
|---|---|---|---|
| `HorizontalLine` | `horizontal-line` | 1 | `LineDrawingOptions` |
| `VerticalLine` | `vertical-line` | 1 | `LineDrawingOptions` |
| `TrendLine` | `trend-line` | 2 | `LineDrawingOptions` |
| `Ray` | `ray` | 2 | `LineDrawingOptions` |
| `Rectangle` | `rectangle` | 2 | `RectangleDrawingOptions` |
| `Text` | `text` | 1 | `TextDrawingOptions` |
| `Note` | `note` | 1 | `TextDrawingOptions` |
| `FibonacciRetracement` | `fibonacci-retracement` | 2 | `FibonacciDrawingOptions` |
| `Measure` | `measure` | 2 | `MeasureDrawingOptions` |
| `LongPosition` | `long-position` | 3 | `PositionDrawingOptions` |
| `ShortPosition` | `short-position` | 3 | `PositionDrawingOptions` |

Die Optionssätze unterscheiden sich nach ihrem Zweck:

- `LineDrawingOptions` — `color`, `lineWidth` (im Bereich (0, 20]), `lineStyle` (0…4).
- `RectangleDrawingOptions` — dasselbe zuzüglich `fillColor` für die Füllung.
- `TextDrawingOptions` — `text` (bis 10 000 Zeichen, Zeilenumbrüche werden berücksichtigt), `color`, `backgroundColor`, `borderColor`, `borderWidth`, `fontSize`, `fontFamily`, `padding`. `Note` unterscheidet sich von `Text` nur durch die Standardwerte: Hintergrund, Rahmen und größere Abstände.
- `FibonacciDrawingOptions` — `levels` (2 bis 32 Werte im Bereich [-5, 5]; Duplikate werden entfernt, die Liste wird sortiert), `labelsVisible`, `extendRight` sowie `color`, `lineWidth`, `lineStyle`, `fillColor`, `fontSize`.
- `MeasureDrawingOptions` — `color`, `lineWidth`, `fillColor`, `labelColor`, `labelBackgroundColor`, `fontSize`. Die Beschriftung zeigt die Kursänderung, den Prozentwert und die Dauer des markierten Intervalls.
- `PositionDrawingOptions` — `entryColor`, `targetColor`, `stopColor`, `targetFillColor`, `stopFillColor`, `textColor`, `lineWidth`, `fontSize` und `quantity`. Die drei Punkte werden der Reihe nach gesetzt: Einstieg, Ziel, Stopp; daraus werden Gewinn, Risiko und das Verhältnis R:R in den Beschriftungen berechnet.

Ein Optionswert, der die Typprüfung nicht besteht, führt zu einer Ausnahme — eine Figur mit fehlerhafter Linienbreite oder leerer Farbe lässt sich nicht speichern.

## Konstruieren mit der Maus

Die schrittweise Eingabe führt der Controller selbst: Er versetzt das Chart in den Zeichenmodus und abonniert Klicks und Fadenkreuz.

```ts
drawings.subscribeCreation(state => {
  if (state === null) return;                    // Konstruktion abgeschlossen oder abgebrochen
  console.log(state.name, state.points.length, state.minimumPoints, state.maximumPoints);
});

drawings.beginCreation(BuiltInDrawingType.Rectangle, { options: { color: '#26a69a' } });

// Abbruch mit Esc, solange die Figur die nötige Punktzahl nicht erreicht hat.
document.addEventListener('keydown', event => {
  if (event.key === 'Escape') drawings.cancelCreation();
});
```

Jeder Klick fügt einen Punkt hinzu, der durch den Magneten läuft; eine Cursorbewegung aktualisiert den Entwurf, der mit demselben Primitiv gezeichnet wird wie die fertige Figur, aber nicht in die Historie gelangt. Das Panel wird mit dem ersten Klick festgelegt, Klicks in anderen Panels werden ignoriert. Sobald so viele Punkte gesetzt sind, wie das Maximum des Typs zulässt, endet die Konstruktion von selbst und es entsteht eine gewöhnliche Figur. `finishCreation` schließt die Konstruktion vorzeitig ab und gibt `null` zurück, wenn weniger Punkte als das Minimum vorliegen; `cancelCreation` verwirft den Entwurf; `creation` liefert die aktuelle Momentaufnahme `DrawingCreationSnapshot`.

## Bindung an die Balken

Der Magnet zieht einen Punkt an die Werte der Serien des aktuellen Panels — gerechnet wird in Bildschirmkoordinaten über den senkrechten Abstand zum Kandidaten.

```ts
import { DrawingMagnetMode } from '@stocksharp/chart/drawings';

const drawings = new DrawingController({
  chart,
  magnet: { mode: DrawingMagnetMode.Weak, maxDistance: 12 },
});

drawings.applyMagnetOptions({ mode: DrawingMagnetMode.Strong });
console.log(drawings.magnetOptions());
```

`DrawingMagnetMode.None` schaltet die Bindung ab, `Weak` (der Standardmodus) zieht nur innerhalb von `maxDistance` an — standardmäßig 10 CSS-Pixel —, `Strong` zieht immer an den nächstgelegenen Wert. Eine Änderung der Einstellungen während der Konstruktion berechnet den Vorschaupunkt sofort neu.

## Speichern und Wiederherstellen

`DrawingInstance` enthält bewusst keine Laufzeitobjekte, deshalb lässt sich ein Markierungssatz unverändert serialisieren:

```ts
import type { DrawingInstance } from '@stocksharp/chart/drawings';

const saved = JSON.stringify(drawings.drawings());

const result = drawings.replaceAll(JSON.parse(saved) as DrawingInstance[], { unknownType: 'skip' });
console.log(result.restored.length, result.skipped);
```

`replaceAll` ersetzt den gesamten Satz vollständig: Zuerst werden alle Eingabeinstanzen geprüft (wiederholte Kennungen sind ein Fehler), dann werden die alten Figuren vom Chart genommen und die neuen hinzugefügt. Kommt auch nur eine Figur nicht zustande, wird der vorherige Zustand wiederhergestellt. Ein unbekannter `type` gelangt bei der Richtlinie `skip` (Standard) mit dem Grund `unknown-type` in `skipped`, bei `error` bricht er die Wiederherstellung ab. Die Wiederherstellung leert die Befehlshistorie, deshalb darf sie nicht innerhalb einer Transaktion aufgerufen werden.

## Eigene Markierungstypen

Der Typkatalog ist erweiterbar. Es genügt, die Definition zu beschreiben und eine Bindung an ein Primitiv zurückzugeben — die fertige Hülle mit Auswahl, Griffen und Ziehen liefert `createInteractiveDrawingBinding`:

```ts
import { createInteractiveDrawingBinding, registerDrawing } from '@stocksharp/chart/drawings';

registerDrawing({
  type: 'price-band',
  name: 'Price Band',
  points: { min: 2, max: 2 },
  defaultOptions: { color: '#4a9eff' },
  normalizeOptions: options => Object.freeze({ color: String(options.color).trim() }),
  create: (instance, events) => createInteractiveDrawingBinding(instance, events, {
    draw(context) {
      const [first, second] = context.points;
      if (second === undefined) return;
      context.context.strokeStyle = String(context.instance.options.color);
      context.context.strokeRect(
        context.plot.x, Math.min(first.y, second.y),
        context.plot.width, Math.abs(second.y - first.y),
      );
    },
    hitTest(point, context) {
      const [first, second] = context.points;
      if (second === undefined) return null;
      return point.y >= Math.min(first.y, second.y) && point.y <= Math.max(first.y, second.y)
        ? { cursor: 'move' }
        : null;
    },
  }),
});

drawings.create('price-band', [
  { time: 1_712_000_000, price: 67_800 },
  { time: 1_712_600_000, price: 68_900 },
]);
```

`draw` erhält die Bildschirmpunkte, das Rechteck des Zeichenbereichs, das Thema, den Skalierungsfaktor und das Auswahlkennzeichen; `hitTest` beantwortet, ob der Cursor den Körper der Figur getroffen hat. Die optionalen `autoscaleInfo` und `handleColor` legen die Beteiligung an der automatischen Skalierung und die Farbe der Griffe fest. `normalizeOptions` wird vor jedem Schreiben in das Modell aufgerufen — das ist die einzige Stelle, an der die Optionswerte geprüft werden sollten.

Das Ziehen des Körpers oder eines einzelnen Punktes läuft über die Ereignisse `preview` (Zwischenzustände, die nicht in die Historie geschrieben werden), `commit` (ein Befehl „Edit drawing“) und `cancel` (Rückkehr zum Zustand vor der Geste). Eine gesperrte Figur (`locked`) lässt sich nicht ziehen und zeigt keine Griffe.

Der Katalog lässt sich auch direkt steuern: `unregisterDrawing(type)`, `getDrawingDefinition(type)`, `getDrawingTypes()`, und `DrawingDefinitionRegistry` erlaubt es, einen eigenen Katalog anzulegen und ihn dem Controller über den Parameter `registry` zu übergeben.

## Öffentliche Methoden

`DrawingController`:

- `drawings()`, `get(id)`, `has(id)` — den aktuellen Satz lesen.
- `create(type, points, options?)`, `add(instance)`, `duplicate(id, duplicateId?)` — Figuren hinzufügen.
- `update(id, patch)`, `updateOptions(id, patch)`, `setVisible(id, visible)`, `setLocked(id, locked)`, `moveToPane(id, paneId)` — ändern.
- `remove(id)`, `clear()` — löschen.
- `beginCreation(type, options?)`, `finishCreation()`, `cancelCreation()`, `creation()` — Konstruktion mit der Maus.
- `magnetOptions()`, `applyMagnetOptions(patch)` — Bindung an die Balken.
- `replaceAll(instances, options?)` — Wiederherstellung eines gespeicherten Satzes.
- `subscribe(listener)` / `unsubscribe(listener)`, `subscribeCreation(listener)` / `unsubscribeCreation(listener)` — Abonnements.
- `dispose()` — Ressourcen freigeben.

Der Konstruktor nimmt `chart` (erforderlich) sowie `registry`, `commandStack`, `idFactory` und `magnet` entgegen.

Der Einstiegspunkt exportiert auch die übrigen Teile der Schicht: `DrawingMagnet` für die eigenständige Berechnung der Bindung, `InteractiveDrawingPrimitive` zusammen mit `createInteractiveDrawingBinding`, die Prüffunktionen `normalizeDrawingInstance` und `normalizeDrawingOptions`, die fertigen Definitionssätze `builtInLineDrawingDefinitions`, `builtInShapeDrawingDefinitions`, `builtInAnalysisDrawingDefinitions`, `builtInPositionDrawingDefinitions` und die dazu passenden Funktionen `registerBuiltInLineDrawings`, `registerBuiltInShapeDrawings`, `registerBuiltInAnalysisDrawings`, `registerBuiltInPositionDrawings` für die Registrierung in einem eigenen Katalog.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Kerzenchart](candlestick.md)
- [Indikatoren](indicators.md)
- [Nachladen der Historie](backfill.md)
