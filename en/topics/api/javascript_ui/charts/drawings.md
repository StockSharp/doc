# Drawing tools

`DrawingController` is the manual annotation layer of the chart: lines, shapes, Fibonacci levels, and position sketches. The controller keeps the shapes as plain JSON objects, binds them to canvas primitives, routes every change through the chart's undo stack, and takes on the step-by-step construction with the mouse.

## Wiring in

The layer ships as a separate entry point of the `@stocksharp/chart` package:

```ts
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';
```

Importing the entry point immediately registers all the built-in drawing types in the shared `drawingDefinitionRegistry` catalog.

## Creating and updating

The controller needs only the chart; by default it takes the command stack from the chart as well (`chart.commandStack()`), so undo and redo work together with the rest of the chart actions:

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';

const chart = createChart(document.getElementById('chart')!, { timeScale: { timeVisible: true } });
chart.addSeries(CandlestickSeries, {}).setData(candles);

const drawings = new DrawingController({ chart });

// A horizontal level: one point, the time is Unix time in seconds.
const level = drawings.create(
  BuiltInDrawingType.HorizontalLine,
  [{ time: 1_712_000_000, price: 68_000 }],
  { options: { color: '#f5c542', lineWidth: 2 } },
);

// A trend line through two points; a sub-pane is named through paneId.
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

// The snapshot of the set arrives sorted by zOrder.
drawings.subscribe(items => console.log(items.length));

chart.commandStack().undo();
```

`create` fills in the missing fields: `paneId` defaults to `main`, `visible` to `true`, `locked` to `false`, and `zOrder` to one above the current maximum, while the options are laid over the `defaultOptions` of the type. `add` inserts a ready instance as a whole, `duplicate` copies an existing one, and `remove` and `clear` delete. Each of these calls puts exactly one undoable command into the history.

`update` changes any combination of fields (`points`, `options`, `paneId`, `visible`, `locked`, `zOrder`); `updateOptions`, `setVisible`, `setLocked`, and `moveToPane` are short forms for the frequent cases. Before being written, an instance is normalized: the points and options are checked for JSON compatibility and frozen, the number of points is verified against the type schema, and the pane against the existing chart panes.

## Built-in types

The identifiers are collected in `BuiltInDrawingType`; the string value is the `type` field of the saved shape.

| Constant | Value | Points | Options |
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

The option sets differ by purpose:

- `LineDrawingOptions` — `color`, `lineWidth` (in the (0, 20] range), `lineStyle` (0…4).
- `RectangleDrawingOptions` — the same plus `fillColor` for the fill.
- `TextDrawingOptions` — `text` (up to 10,000 characters, line breaks honoured), `color`, `backgroundColor`, `borderColor`, `borderWidth`, `fontSize`, `fontFamily`, `padding`. `Note` differs from `Text` only in its defaults: a background, a border, and larger padding.
- `FibonacciDrawingOptions` — `levels` (from 2 to 32 values in the [-5, 5] range; duplicates are removed and the list is sorted), `labelsVisible`, `extendRight`, and also `color`, `lineWidth`, `lineStyle`, `fillColor`, `fontSize`.
- `MeasureDrawingOptions` — `color`, `lineWidth`, `fillColor`, `labelColor`, `labelBackgroundColor`, `fontSize`. The label shows the price change, the percentage, and the duration of the selected interval.
- `PositionDrawingOptions` — `entryColor`, `targetColor`, `stopColor`, `targetFillColor`, `stopFillColor`, `textColor`, `lineWidth`, `fontSize`, and `quantity`. The three points are given in order: entry, target, stop; the profit, the risk, and the R:R ratio in the labels are computed from them.

An option value that fails the type check raises an exception — a shape with an invalid line width or an empty color cannot be saved.

## Building with the mouse

The step-by-step input is driven by the controller itself: it puts the chart into drawing mode and subscribes to the clicks and the crosshair.

```ts
drawings.subscribeCreation(state => {
  if (state === null) return;                    // the construction is finished or cancelled
  console.log(state.name, state.points.length, state.minimumPoints, state.maximumPoints);
});

drawings.beginCreation(BuiltInDrawingType.Rectangle, { options: { color: '#26a69a' } });

// Cancel with Esc while the shape has not collected the required number of points.
document.addEventListener('keydown', event => {
  if (event.key === 'Escape') drawings.cancelCreation();
});
```

Every click adds a point passed through the magnet; cursor movement updates the draft, which is drawn by the same primitive as the finished shape but never enters the history. The pane is fixed by the first click, and clicks in other panes are ignored. As soon as as many points are collected as the maximum of the type allows, the construction finishes by itself and an ordinary shape is created. `finishCreation` closes the construction early and returns `null` when there are fewer points than the minimum; `cancelCreation` drops the draft; `creation` gives out the current `DrawingCreationSnapshot`.

## Snapping to bars

The magnet pulls a point towards the values of the series of the current pane — the computation runs in screen coordinates, by the vertical distance to a candidate.

```ts
import { DrawingMagnetMode } from '@stocksharp/chart/drawings';

const drawings = new DrawingController({
  chart,
  magnet: { mode: DrawingMagnetMode.Weak, maxDistance: 12 },
});

drawings.applyMagnetOptions({ mode: DrawingMagnetMode.Strong });
console.log(drawings.magnetOptions());
```

`DrawingMagnetMode.None` turns the snapping off, `Weak` (the default mode) pulls only within `maxDistance` — 10 CSS pixels by default, and `Strong` always pulls to the nearest value. Changing the settings during construction recomputes the preview point immediately.

## Saving and restoring

`DrawingInstance` deliberately contains no runtime objects, so a set of drawings is serialized as it is:

```ts
import type { DrawingInstance } from '@stocksharp/chart/drawings';

const saved = JSON.stringify(drawings.drawings());

const result = drawings.replaceAll(JSON.parse(saved) as DrawingInstance[], { unknownType: 'skip' });
console.log(result.restored.length, result.skipped);
```

`replaceAll` replaces the whole set: first all the input instances are validated (duplicate identifiers are an error), then the old shapes are taken off the chart and the new ones are added. If at least one shape does not go in, the previous state is restored. An unknown `type` under the `skip` policy (the default) lands in `skipped` with the `unknown-type` reason, and under `error` it aborts the restoration. Restoration clears the command history, so it cannot be called inside a transaction.

## Your own drawing types

The type catalog is extensible. It is enough to describe a definition and return a primitive binding — the ready wrapper with selection, handles, and dragging is given by `createInteractiveDrawingBinding`:

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

`draw` receives the screen points, the rectangle of the plot area, the theme, the scaling factor, and the selected flag; `hitTest` answers whether the cursor landed inside the body of the shape. The optional `autoscaleInfo` and `handleColor` set the participation in autoscaling and the color of the handles. `normalizeOptions` is called before every write into the model — it is the only place where option values are worth checking.

Dragging the body or a single point goes through the `preview` (intermediate states, never written to the history), `commit` (a single "Edit drawing" command), and `cancel` (return to the state before the gesture) events. A locked shape (`locked`) is neither dragged nor shows handles.

The catalog can be driven directly as well: `unregisterDrawing(type)`, `getDrawingDefinition(type)`, `getDrawingTypes()`, and `DrawingDefinitionRegistry` allows a separate catalog to be created and passed to the controller through the `registry` parameter.

## Public methods

`DrawingController`:

- `drawings()`, `get(id)`, `has(id)` — reading the current set.
- `create(type, points, options?)`, `add(instance)`, `duplicate(id, duplicateId?)` — adding shapes.
- `update(id, patch)`, `updateOptions(id, patch)`, `setVisible(id, visible)`, `setLocked(id, locked)`, `moveToPane(id, paneId)` — changing them.
- `remove(id)`, `clear()` — deleting them.
- `beginCreation(type, options?)`, `finishCreation()`, `cancelCreation()`, `creation()` — building with the mouse.
- `magnetOptions()`, `applyMagnetOptions(patch)` — snapping to bars.
- `replaceAll(instances, options?)` — restoring a saved set.
- `subscribe(listener)` / `unsubscribe(listener)`, `subscribeCreation(listener)` / `unsubscribeCreation(listener)` — subscriptions.
- `dispose()` — release resources.

The constructor takes `chart` (mandatory), and also `registry`, `commandStack`, `idFactory`, and `magnet`.

The entry point exports the other parts of the layer too: `DrawingMagnet` for computing the snapping on your own, `InteractiveDrawingPrimitive` together with `createInteractiveDrawingBinding`, the `normalizeDrawingInstance` and `normalizeDrawingOptions` validation functions, the ready definition sets `builtInLineDrawingDefinitions`, `builtInShapeDrawingDefinitions`, `builtInAnalysisDrawingDefinitions`, `builtInPositionDrawingDefinitions` and their paired functions `registerBuiltInLineDrawings`, `registerBuiltInShapeDrawings`, `registerBuiltInAnalysisDrawings`, `registerBuiltInPositionDrawings` for registering them in a catalog of your own.

## See also

- [JavaScript charts](../charts.md)
- [Candlestick](candlestick.md)
- [Indicators](indicators.md)
- [History backfill](backfill.md)
