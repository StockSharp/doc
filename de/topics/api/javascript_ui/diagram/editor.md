# Interaktiver Editor

Die schreibgeschützte Einbettung (`renderScheme`) ist eine dünne Hülle um den vollständigen Editor. Die Klasse `StockSharpDiagram` — zusammen mit `StockSharpPalette` und `StockSharpCatalog`, die alle aus `@stocksharp/diagram` exportiert werden — ist ein vollwertiger visueller Editor: Ziehen Sie Elemente aus der Palette hinein, verbinden Sie Ports, verschieben und löschen Sie Knoten, machen Sie Aktionen rückgängig bzw. wieder her (Undo/Redo) und nutzen Sie die typisierte Verbindungsvalidierung. Die Bearbeitung ist standardmäßig aktiviert.

## Live-Demo

Ziehen Sie ein Element aus der Palette auf die Arbeitsfläche, ziehen Sie zwischen den Ports, um sie zu verbinden, klicken Sie mit der rechten Maustaste für das Menü und verwenden Sie Undo/Redo. Inkompatible Verbindungen werden abgelehnt (beobachten Sie die Statuszeile). Drücken Sie **Error**, um einen animierten Laufzeitfehler an einem Knoten aufblinken zu lassen (siehe [Ereignisse und API](events.md)).

```diagram-editor sma
```

## Einrichtung

Erstellen Sie einen **Katalog** aus Port- und Elementtypen und leiten Sie daraus dann das Diagramm und die Palette ab:

```js
import {
  StockSharpDiagram, StockSharpCatalog, StockSharpPalette,
  Node, PortType, DiagramNode, Link, PALETTE_DRAG_MIME,
} from '@stocksharp/diagram';

// 1) Katalog: die Sockel- (Port-) Typen und die Element- (Knoten-) Typen.
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

// 2) Bearbeitbares Diagramm + Paletten-Werkzeugkasten (jeweils in ihr eigenes Element gerendert).
const canvasHost = document.getElementById('diagram');
const paletteHost = document.getElementById('palette');

const diagram = new StockSharpDiagram({ div: canvasHost, catalog, showFullscreenButton: true });
const palette = new StockSharpPalette({ div: paletteHost, catalog });

// 3) Knoten aus der Palette hinzufügen: Ein Doppelklick legt den Knoten in die Mitte des sichtbaren Bereichs.
palette.on('nodeActivated', ({ node }) => {
  const box = canvasHost.getBoundingClientRect();
  diagram.dropNodeFromPalette(node.id, box.left + box.width / 2, box.top + box.height / 2);
});

// Natives Drag-and-Drop auf die Arbeitsfläche. dragover ist zwingend nötig: Ohne das Abbrechen dieses
// Ereignisses betrachtet der Browser das Element nicht als Ziel, und drop findet überhaupt nicht statt.
canvasHost.addEventListener('dragover', event => event.preventDefault());
canvasHost.addEventListener('drop', event => {
  event.preventDefault();
  const payload = event.dataTransfer?.getData(PALETTE_DRAG_MIME);   // dataTransfer kann null sein
  if (!payload) return;

  const { typeId } = JSON.parse(payload);
  if (typeId) diagram.dropNodeFromPalette(typeId, event.clientX, event.clientY);
});

// 4) Ein Startschema laden und auf Bearbeitungen reagieren.
diagram.load(
  [new DiagramNode({ id: 'c', typeId: 'candles', name: 'Candles', x: 60, y: 120 })],
  [],
);
diagram.on('linkValidation', ({ allowed, reason }) => { if (!allowed) console.log('abgelehnt:', reason); });
diagram.zoomToFit();
```

Anstatt den Katalog von Hand aufzubauen, können Sie ihm eine Paletten-JSON übergeben (dieselbe `designer-palette.json`, die auch die schreibgeschützte Einbettung abruft), wobei `socketTypes` in `PortType`s und `elements` in `Node`s umgewandelt werden.

Um die schreibgeschützte Einbettung in einen Editor zu verwandeln, ohne sie neu aufzubauen, greifen Sie über den zurückgegebenen Handle zu — `.diagram` ist die vollständige Instanz:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // Bearbeitung aktiviert
```

## Siehe auch

- [JavaScript-Diagramm](../diagram.md)
- [Ereignisse und API](events.md)
- [JavaScript-Charts](../charts.md)
