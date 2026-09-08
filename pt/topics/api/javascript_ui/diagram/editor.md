# Editor interativo

O embed só de leitura (`renderScheme`) é uma camada leve sobre o editor completo. A classe `StockSharpDiagram` — juntamente com `StockSharpPalette` e `StockSharpCatalog`, todas exportadas por `@stocksharp/diagram` — constitui um editor visual completo: arraste elementos da paleta, ligue portas, mova e elimine nós, anule ou refaça operações e valide ligações tipadas. A edição está ativada por predefinição.

## Demonstração em direto

Arraste um elemento da paleta para o `canvas`, arraste entre portas para as ligar, clique com o botão direito para abrir o menu e utilize **Anular/Refazer**. As ligações incompatíveis são rejeitadas (consulte a linha de estado). Prima **Error** para apresentar um erro animado de tempo de execução num nó (consulte [Eventos e API](events.md)).

```diagram-editor sma
```

## Configuração

Monte um **catálogo** de tipos de portas e de elementos e, em seguida, crie o diagrama e a paleta a partir dele:

```js
import {
  StockSharpDiagram, StockSharpCatalog, StockSharpPalette,
  Node, PortType, DiagramNode, Link, PALETTE_DRAG_MIME,
} from '@stocksharp/diagram';

// 1) Catálogo: os tipos de soquete (porta) e os tipos de elemento (nó).
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

// 2) Diagrama editável + caixa de ferramentas da paleta (cada um é apresentado no respetivo elemento).
const canvasHost = document.getElementById('diagram');
const paletteHost = document.getElementById('palette');

const diagram = new StockSharpDiagram({ div: canvasHost, catalog, showFullscreenButton: true });
const palette = new StockSharpPalette({ div: paletteHost, catalog });

// 3) Adicione nós a partir da paleta: o duplo clique coloca o nó no centro da área visível.
palette.on('nodeActivated', ({ node }) => {
  const box = canvasHost.getBoundingClientRect();
  diagram.dropNodeFromPalette(node.id, box.left + box.width / 2, box.top + box.height / 2);
});

// Arrastar/soltar nativo no canvas. O dragover é obrigatório: sem cancelar este evento,
// o navegador não considera o elemento um recetor e o drop nunca acontece.
canvasHost.addEventListener('dragover', event => event.preventDefault());
canvasHost.addEventListener('drop', event => {
  event.preventDefault();
  const payload = event.dataTransfer?.getData(PALETTE_DRAG_MIME);   // dataTransfer pode ser null
  if (!payload) return;

  const { typeId } = JSON.parse(payload);
  if (typeId) diagram.dropNodeFromPalette(typeId, event.clientX, event.clientY);
});

// 4) Carregue um esquema inicial e reaja às edições.
diagram.load(
  [new DiagramNode({ id: 'c', typeId: 'candles', name: 'Candles', x: 60, y: 120 })],
  [],
);
diagram.on('linkValidation', ({ allowed, reason }) => { if (!allowed) console.log('rejeitado:', reason); });
diagram.zoomToFit();
```

Em vez de montar o catálogo manualmente, pode alimentá-lo com um JSON de paleta (o mesmo `designer-palette.json` que o embed só de leitura obtém), convertendo `socketTypes` em `PortType`s e `elements` em `Node`s.

Para transformar o embed só de leitura num editor sem o reconstruir, utilize o objeto devolvido — `.diagram` é a instância completa:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // edição ativada
```

## Veja também

- [Diagrama em JavaScript](../diagram.md)
- [Eventos e API](events.md)
- [Gráficos em JavaScript](../charts.md)
