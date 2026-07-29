# Editor interactivo

La incrustación de solo lectura (`renderScheme`) es una envoltura ligera sobre el editor completo. La clase `StockSharpDiagram` — junto con `StockSharpPalette` y `StockSharpCatalog`, todas exportadas desde `@stocksharp/diagram` — es un editor visual completo que permite arrastrar elementos desde la paleta, conectar puertos, mover y eliminar nodos, deshacer/rehacer y validar enlaces con tipos. La edición está activada de forma predeterminada.

## Demo en vivo

Arrastre un elemento desde la paleta hacia el lienzo, arrastre entre puertos para conectarlos, haga clic derecho para abrir el menú y use Deshacer/Rehacer. Las conexiones incompatibles se rechazan (observe la línea de estado). Pulse **Error** para mostrar un error de ejecución animado en un nodo (consulte [Eventos y API](events.md)).

```diagram-editor sma
```

## Configuración

Cree un **catálogo** de tipos de puertos y de elementos, y luego genere el diagrama y la paleta a partir de él:

```js
import {
  StockSharpDiagram, StockSharpCatalog, StockSharpPalette,
  Node, PortType, DiagramNode, Link, PALETTE_DRAG_MIME,
} from '@stocksharp/diagram';

// 1) Catálogo: los tipos de conector (puerto) y los tipos de elemento (nodo).
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

// 2) Diagrama editable + caja de herramientas de la paleta (cada uno se renderiza en su propio elemento).
const diagram = new StockSharpDiagram({ div: canvasHost, catalog, showFullscreenButton: true });
const palette = new StockSharpPalette({ div: paletteHost, catalog });

// 3) Añada nodos desde la paleta: doble clic, o arrastrar y soltar nativo sobre el lienzo.
palette.on('nodeActivated', ({ node }) => diagram.dropNodeFromPalette(node.id, centerX, centerY));
canvasHost.addEventListener('drop', event => {
  const { typeId } = JSON.parse(event.dataTransfer.getData(PALETTE_DRAG_MIME) || '{}');
  if (typeId) diagram.dropNodeFromPalette(typeId, event.clientX, event.clientY);
});

// 4) Cargue un esquema inicial y reaccione a las ediciones.
diagram.load(
  [new DiagramNode({ id: 'c', typeId: 'candles', name: 'Candles', x: 60, y: 120 })],
  [],
);
diagram.on('linkValidation', ({ allowed, reason }) => { if (!allowed) console.log('rechazado:', reason); });
diagram.zoomToFit();
```

En lugar de construir el catálogo a mano, puede alimentarlo con un JSON de paleta (el mismo `designer-palette.json` que obtiene la incrustación de solo lectura), convirtiendo los `socketTypes` en `PortType` y los `elements` en `Node`.

Para convertir la incrustación de solo lectura en un editor sin volver a crearla, acceda mediante el manejador que devuelve — `.diagram` es la instancia completa:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // edición activada
```

## Véase también

- [Diagrama en JavaScript](../javascript_diagram.md)
- [Eventos y API](events.md)
- [Gráficos en JavaScript](../charts/javascript_charts.md)
