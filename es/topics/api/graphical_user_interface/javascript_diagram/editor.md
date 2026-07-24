# Editor interactivo

La incrustación de solo lectura (`renderScheme`) es una envoltura ligera sobre el editor completo. La clase `StockSharpDiagram` — junto con `StockSharpPalette` y `StockSharpCatalog`, todas exportadas desde `@stocksharp/diagram` — es un editor visual completo: arrastra elementos desde la paleta, conecta puertos, mueve y elimina nodos, deshaz/rehaz y valida enlaces con tipos. La edición está activada de forma predeterminada.

## Demo en vivo

Arrastra un elemento desde la paleta hacia el lienzo, arrastra entre puertos para conectarlos, haz clic derecho para abrir el menú y usa Deshacer/Rehacer. Las conexiones incompatibles se rechazan (observa la línea de estado). Pulsa **Error** para mostrar un error de ejecución animado en un nodo (consulta [Eventos y API](events.md#runtime-state-and-error-highlighting)).

```diagram-editor sma
```

## Configuración

Crea un **catálogo** de tipos de puertos y de elementos, y luego genera el diagrama y la paleta a partir de él:

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

En lugar de construir el catálogo a mano, puedes alimentarlo con un JSON de paleta (el mismo `designer-palette.json` que obtiene la incrustación de solo lectura), convirtiendo los `socketTypes` en `PortType` y los `elements` en `Node`.

Para convertir la incrustación de solo lectura en un editor sin volver a crearla, accede mediante el manejador que devuelve — `.diagram` es la instancia completa:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // editing enabled
```

## Véase también

- [Diagrama JavaScript](../javascript_diagram.md)
- [Eventos y API](events.md)
- [Gráficos JavaScript](../charts/javascript_charts.md)
