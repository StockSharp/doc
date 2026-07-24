# Editor interativo

O embed somente leitura (`renderScheme`) é um wrapper fino sobre o editor completo. A classe `StockSharpDiagram` — junto com `StockSharpPalette` e `StockSharpCatalog`, todas exportadas de `@stocksharp/diagram` — é um editor visual completo: arraste elementos da paleta, conecte portas, mova e exclua nós, desfaça/refaça e valide links tipados. A edição fica ativada por padrão.

## Demonstração ao vivo

Arraste um elemento da paleta para o canvas, arraste entre portas para conectá-las, clique com o botão direito para abrir o menu e use Desfazer/Refazer. Conexões incompatíveis são rejeitadas (observe a linha de status). Pressione **Error** para exibir um erro de tempo de execução animado em um nó (veja [Eventos e API](events.md#runtime-state-and-error-highlighting)).

```diagram-editor sma
```

## Configuração

Monte um **catálogo** de tipos de portas e de elementos e, em seguida, crie o diagrama e a paleta a partir dele:

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

Em vez de montar o catálogo manualmente, você pode alimentá-lo com um JSON de paleta (o mesmo `designer-palette.json` que o embed somente leitura busca), convertendo `socketTypes` em `PortType`s e `elements` em `Node`s.

Para transformar o embed somente leitura em um editor sem reconstruí-lo, use o handle que ele retorna — `.diagram` é a instância completa:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // editing enabled
```

## Veja também

- [Diagrama JavaScript](../javascript_diagram.md)
- [Eventos e API](events.md)
- [Gráficos JavaScript](../charts/javascript_charts.md)
