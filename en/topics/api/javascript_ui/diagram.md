# JavaScript diagram

[StockSharp JS Diagram](https://github.com/StockSharp/JS-Diagram) is a standalone, dependency-free browser component that renders the visual strategy scheme of [Designer](../../designer.md) — the same block diagram of connected elements — on an HTML `canvas`. It is published on npm as [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) and powers the read-only strategy diagrams shown across the StockSharp web sites.

A strategy is described as a **scheme**: a set of *nodes* (elements such as a candle source, an indicator, a condition or an order) wired together through typed *ports*. The component takes that scheme plus a *palette* (the catalog of element types, their ports and colors) and draws it.

## Live demo

The diagram below is the real engine running on this page — a minimal "data source → indicator → chart" strategy skeleton. Drag the canvas to pan, use the wheel to zoom, and press the expand button to open it full screen.

```diagram-demo sma
```

The three blocks are a **Candles** source feeding an **Indicator** (a simple moving average); both the candles and the indicator output are drawn on a **Chart** element. This is the smallest complete pattern in Designer: produce data, transform it, visualize it.

## Installation

Install the package from npm:

```bash
npm install @stocksharp/diagram
```

Then import the ES modules — `import { renderScheme } from '@stocksharp/diagram/embed'` for the read-only embed, or `import { StockSharpDiagram } from '@stocksharp/diagram'` for the [interactive editor](diagram/editor.md).

## Embedding a diagram

The component exposes `renderScheme(host, paletteUrl, scheme)` from the `@stocksharp/diagram/embed` entry. Give it a host element, the URL of a palette JSON, and a scheme built from `nodes` and `links`:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const scheme = {
  nodes: [
    { id: 'candles', typeId: 'CandleElement',    name: 'Candles', x: 60,  y: 130 },
    { id: 'sma',     typeId: 'IndicatorElement', name: 'SMA',     x: 340, y: 60  },
    { id: 'chart',   typeId: 'ChartElement',     name: 'Chart',   x: 620, y: 130 },
  ],
  links: [
    { from: 'candles', fromPort: 'Output', to: 'sma',   toPort: 'Input' },
    { from: 'sma',     fromPort: 'Output', to: 'chart', toPort: 'Input' },
    { from: 'candles', fromPort: 'Output', to: 'chart', toPort: 'Input' },
  ],
};

renderScheme(document.getElementById('diagram'), '/data/designer-palette.json', scheme);
```

Each node's `typeId` must exist in the palette; unknown types render as placeholder blocks. Ports are referenced by their `key`, and a link is valid when the source port's type is compatible with the target port's type. `renderScheme` is read-only: the engine lays out, themes (it follows the page's light/dark setting) and lets the viewer pan, zoom and expand, but does not edit the scheme.

The same component also runs as a full **editor** — drag elements from a palette, connect ports, edit and delete nodes, undo/redo. See [Interactive editor](diagram/editor.md) and [Events and API](diagram/events.md).

## See also

- [Interactive editor](diagram/editor.md)
- [Events and API](diagram/events.md)
- [JavaScript charts](charts.md)
- [Designer](../../designer.md) — the desktop visual strategy editor
- [JS-Diagram repository](https://github.com/StockSharp/JS-Diagram)
