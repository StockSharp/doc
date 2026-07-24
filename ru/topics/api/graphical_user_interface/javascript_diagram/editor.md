# Интерактивный редактор

Read-only-встраивание (`renderScheme`) — это тонкая обёртка над полноценным редактором. Класс `StockSharpDiagram` — вместе с `StockSharpPalette` и `StockSharpCatalog`, экспортируемыми из `@stocksharp/diagram` — это полный визуальный редактор: перетаскивание элементов из палитры, соединение портов, перемещение и удаление узлов, undo/redo и типизированная валидация связей. Редактирование включено по умолчанию.

## Живая демонстрация

Перетащите элемент из палитры на холст, протяните связь между портами, вызовите меню правым кликом, используйте Undo/Redo. Несовместимые соединения отклоняются (следите за строкой статуса). Нажмите **Error**, чтобы подсветить узел анимированной ошибкой выполнения (см. [События и API](events.md)).

```diagram-editor sma
```

## Настройка

Соберите **каталог** типов портов и элементов, затем создайте из него диаграмму и палитру:

```js
import {
  StockSharpDiagram, StockSharpCatalog, StockSharpPalette,
  Node, PortType, DiagramNode, Link, PALETTE_DRAG_MIME,
} from '@stocksharp/diagram';

// 1) Каталог: типы сокетов (портов) и типы элементов (узлов).
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

// 2) Редактируемая диаграмма + палитра инструментов (каждая рендерится в свой элемент).
const diagram = new StockSharpDiagram({ div: canvasHost, catalog, showFullscreenButton: true });
const palette = new StockSharpPalette({ div: paletteHost, catalog });

// 3) Добавление узлов из палитры: двойной клик или нативный drag/drop на холст.
palette.on('nodeActivated', ({ node }) => diagram.dropNodeFromPalette(node.id, centerX, centerY));
canvasHost.addEventListener('drop', event => {
  const { typeId } = JSON.parse(event.dataTransfer.getData(PALETTE_DRAG_MIME) || '{}');
  if (typeId) diagram.dropNodeFromPalette(typeId, event.clientX, event.clientY);
});

// 4) Загрузка начальной схемы и реакция на правки.
diagram.load(
  [new DiagramNode({ id: 'c', typeId: 'candles', name: 'Candles', x: 60, y: 120 })],
  [],
);
diagram.on('linkValidation', ({ allowed, reason }) => { if (!allowed) console.log('отклонено:', reason); });
diagram.zoomToFit();
```

Вместо построения каталога вручную можно передать ему палитру-JSON (тот самый `designer-palette.json`, который подгружает read-only-встраивание), преобразовав `socketTypes` в `PortType`, а `elements` — в `Node`.

Чтобы превратить read-only-встраивание в редактор без пересборки, обратитесь через возвращаемый им хэндл — поле `.diagram` содержит полноценный экземпляр:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // редактирование включено
```

## Смотрите также

- [JavaScript-диаграмма](../javascript_diagram.md)
- [События и API](events.md)
- [JavaScript-графики](../charts/javascript_charts.md)
