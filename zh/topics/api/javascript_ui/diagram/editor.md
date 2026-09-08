# 交互式编辑器

只读嵌入（`renderScheme`）只是完整编辑器之上的一层轻量封装。`StockSharpDiagram` 类——连同 `StockSharpPalette` 和 `StockSharpCatalog`，均从 `@stocksharp/diagram` 导出——是一个完整的可视化编辑器：从调色板拖入元素、连接端口、移动和删除节点、撤销/重做，以及带类型校验的连线。编辑功能默认开启。

## 在线演示

从调色板将元素拖到画布上，在端口之间拖动以建立连接，右键打开菜单，并使用撤销/重做。不兼容的连接会被拒绝（请留意状态栏）。按下 **Error** 可在节点上闪现一个动画运行时错误（参见 [事件与 API](events.md)）。

```diagram-editor sma
```

## 配置

先构建一个包含端口类型和元素类型的**目录（catalog）**，然后基于它创建图表和调色板：

```js
import {
  StockSharpDiagram, StockSharpCatalog, StockSharpPalette,
  Node, PortType, DiagramNode, Link, PALETTE_DRAG_MIME,
} from '@stocksharp/diagram';

// 1) 目录（catalog）：套接字（端口）类型与元素（节点）类型。
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

// 2) 可编辑的框图 + 调色板工具箱（各自渲染到自己的元素中）。
const canvasHost = document.getElementById('diagram');
const paletteHost = document.getElementById('palette');

const diagram = new StockSharpDiagram({ div: canvasHost, catalog, showFullscreenButton: true });
const palette = new StockSharpPalette({ div: paletteHost, catalog });

// 3) 从调色板添加节点：双击会把节点放到可见区域的中心。
palette.on('nodeActivated', ({ node }) => {
  const box = canvasHost.getBoundingClientRect();
  diagram.dropNodeFromPalette(node.id, box.left + box.width / 2, box.top + box.height / 2);
});

// 原生拖放到画布上。dragover 是必需的：不取消该事件，
// 浏览器就不会把该元素视为放置目标，drop 根本不会发生。
canvasHost.addEventListener('dragover', event => event.preventDefault());
canvasHost.addEventListener('drop', event => {
  event.preventDefault();
  const payload = event.dataTransfer?.getData(PALETTE_DRAG_MIME);   // dataTransfer 可能为 null
  if (!payload) return;

  const { typeId } = JSON.parse(payload);
  if (typeId) diagram.dropNodeFromPalette(typeId, event.clientX, event.clientY);
});

// 4) 加载初始方案并响应编辑。
diagram.load(
  [new DiagramNode({ id: 'c', typeId: 'candles', name: 'Candles', x: 60, y: 120 })],
  [],
);
diagram.on('linkValidation', ({ allowed, reason }) => { if (!allowed) console.log('已拒绝：', reason); });
diagram.zoomToFit();
```

除了手动构建目录之外，你也可以给它提供一份调色板 JSON（即只读嵌入所获取的那份 `designer-palette.json`），将其中的 `socketTypes` 转换为 `PortType`，将 `elements` 转换为 `Node`。

若想把只读嵌入直接变成编辑器而无需重新构建，可通过它返回的句柄进行操作——`.diagram` 就是完整的实例：

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // 已启用编辑
```

## 另请参阅

- [JavaScript 框图](../diagram.md)
- [事件与 API](events.md)
- [JavaScript 图表](../charts.md)
