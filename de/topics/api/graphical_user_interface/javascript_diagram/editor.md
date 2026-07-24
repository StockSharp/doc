# Interaktiver Editor

Die schreibgeschützte Einbettung (`renderScheme`) ist ein dünner Wrapper um den vollständigen Editor. Die Klasse `StockSharpDiagram` — zusammen mit `StockSharpPalette` und `StockSharpCatalog`, die alle aus `@stocksharp/diagram` exportiert werden — ist ein vollwertiger visueller Editor: Ziehen Sie Elemente aus der Palette hinein, verbinden Sie Ports, verschieben und löschen Sie Knoten, machen Sie Aktionen rückgängig bzw. wieder her (Undo/Redo) und nutzen Sie die typisierte Verbindungsvalidierung. Die Bearbeitung ist standardmäßig aktiviert.

## Live-Demo

Ziehen Sie ein Element aus der Palette auf die Arbeitsfläche, ziehen Sie zwischen den Ports, um sie zu verbinden, klicken Sie mit der rechten Maustaste für das Menü und verwenden Sie Undo/Redo. Inkompatible Verbindungen werden abgelehnt (beobachten Sie die Statuszeile). Drücken Sie **Error**, um einen animierten Laufzeitfehler an einem Knoten aufblinken zu lassen (siehe [Ereignisse und API](events.md#runtime-state-and-error-highlighting)).

```diagram-editor sma
```

## Einrichtung

Erstellen Sie einen **Katalog** aus Port- und Elementtypen und leiten Sie daraus dann das Diagramm und die Palette ab:

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

Anstatt den Katalog von Hand aufzubauen, können Sie ihm eine Paletten-JSON übergeben (dieselbe `designer-palette.json`, die auch die schreibgeschützte Einbettung abruft), wobei `socketTypes` in `PortType`s und `elements` in `Node`s umgewandelt werden.

Um die schreibgeschützte Einbettung in einen Editor zu verwandeln, ohne sie neu aufzubauen, greifen Sie über den zurückgegebenen Handle zu — `.diagram` ist die vollständige Instanz:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // editing enabled
```

## Siehe auch

- [JavaScript-Diagramm](../javascript_diagram.md)
- [Ereignisse und API](events.md)
- [JavaScript-Charts](../charts/javascript_charts.md)
