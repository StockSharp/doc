# Interactive editor

The read-only embed (`renderScheme`) is a thin wrapper over the full editor. The `StockSharpDiagram` class — with `StockSharpPalette` and `StockSharpCatalog`, all exported from `@stocksharp/diagram` — is a complete visual editor: drag elements in from the palette, connect ports, move and delete nodes, undo/redo, and typed link validation. Editing is on by default.

## Live demo

Drag an element from the palette onto the canvas, drag between ports to connect them, right-click for the menu, and use Undo/Redo. Incompatible connections are rejected (watch the status line). Press **Error** to flash an animated runtime error on a node (see [Events and API](events.md)).

```diagram-editor sma
```

## Setup

Build a **catalog** of port and element types, then create the diagram and the palette from it:

```js
import {
  StockSharpDiagram, StockSharpCatalog, StockSharpPalette,
  Node, PortType, DiagramNode, Link, PALETTE_DRAG_MIME,
} from '@stocksharp/diagram';

// 1) Catalog: the socket (port) types and the element (node) types.
const catalog = new StockSharpCatalog();
catalog.addPortType(new PortType({ name: 'Candle', color: '#4aa3ff' }));
catalog.addPortType(new PortType({ name: 'Indicator', color: '#a779e9' }));
catalog.addNodeType(new Node({
  id: 'candles', name: 'Candles', groupName: 'Sources',
  outPorts: [{ id: 'Output', name: 'Output', type: 'Candle' }],
}));
catalog.addNodeType(new Node({
  id: 'sma', name: 'SMA', groupName: 'Indicators',
  inPorts: [{ id: 'Input', name: 'Input', type: 'Candle', maxLinks: 1 }],
  outPorts: [{ id: 'Output', name: 'Output', type: 'Indicator' }],
}));

// 2) Editable diagram + palette toolbox (each renders into its own element).
const diagram = new StockSharpDiagram({ div: canvasHost, catalog, showFullscreenButton: true });
const palette = new StockSharpPalette({ div: paletteHost, catalog });

// 3) Add nodes from the palette: double-click, or native drag/drop onto the canvas.
palette.on('nodeActivated', ({ node }) => diagram.dropNodeFromPalette(node.id, centerX, centerY));
canvasHost.addEventListener('drop', event => {
  const { typeId } = JSON.parse(event.dataTransfer.getData(PALETTE_DRAG_MIME) || '{}');
  if (typeId) diagram.dropNodeFromPalette(typeId, event.clientX, event.clientY);
});

// 4) Load a starting scheme and react to edits.
diagram.load(
  [new DiagramNode({ id: 'c', typeId: 'candles', name: 'Candles', x: 60, y: 120 })],
  [],
);
diagram.on('linkValidation', ({ allowed, reason }) => { if (!allowed) console.log('rejected:', reason); });
diagram.zoomToFit();
```

Instead of building the catalog by hand you can feed it a palette JSON (the same `designer-palette.json` the read-only embed fetches), converting `socketTypes` into `PortType`s and `elements` into `Node`s.

To turn the read-only embed into an editor without rebuilding it, reach through the handle it returns — `.diagram` is the full instance:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // editing enabled
```

## See also

- [JavaScript diagram](../diagram.md)
- [Events and API](events.md)
- [JavaScript charts](../charts.md)
