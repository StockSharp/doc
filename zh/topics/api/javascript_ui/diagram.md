# JavaScript 框图

[StockSharp JS Diagram](https://github.com/StockSharp/JS-Diagram) 是一个独立、无依赖的浏览器组件，用于在 HTML `canvas` 上渲染 [Designer](../../designer.md) 的可视化策略方案——即由相互连接的元素构成的同一张框图。它以 [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) 的形式发布在 npm 上，为 StockSharp 各网站中展示的只读策略框图提供支持。

一个策略被描述为一个**方案（scheme）**：一组*节点（nodes）*（诸如 K线 数据源、指标、条件或订单之类的元素）通过带类型的*端口（ports）*相互连接。该组件接收这个方案以及一个*调色板（palette）*（元素类型、其端口及颜色的目录），并将其绘制出来。

## 在线演示

下方的框图就是运行在本页面上的真实引擎——一个极简的“数据源 → 指标 → 图表”策略骨架。拖动画布可平移，使用滚轮可缩放，按下展开按钮可全屏打开。

```diagram-demo sma
```

这三个块分别是：一个为**指标**（一条简单移动平均线）供给数据的**K线**数据源；K线和指标的输出都被绘制在一个**图表**元素上。这是 Designer 中最小的完整模式：产生数据、对其进行转换、再将其可视化。

## 安装

从 npm 安装该包：

```bash
npm install @stocksharp/diagram
```

然后导入 ES 模块——只读嵌入使用 `import { renderScheme } from '@stocksharp/diagram/embed'`，[交互式编辑器](diagram/editor.md)则使用 `import { StockSharpDiagram } from '@stocksharp/diagram'`。

## 嵌入框图

该组件从 `@stocksharp/diagram/embed` 入口暴露出 `renderScheme(host, paletteUrl, scheme)`。给它传入一个宿主元素、一个调色板 JSON 的 URL，以及一个由 `nodes` 和 `links` 构建的方案：

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

每个节点的 `typeId` 都必须存在于调色板中；未知类型将渲染为占位块。端口通过其 `key` 来引用，当源端口的类型与目标端口的类型兼容时，一条连接才有效。`renderScheme` 是只读的：引擎负责布局、应用主题（它跟随页面的明暗设置）并允许查看者平移、缩放和展开，但不会编辑方案。

同一组件也可以作为一个功能完整的**编辑器**运行——从调色板拖出元素、连接端口、编辑和删除节点、撤销/重做。参见[交互式编辑器](diagram/editor.md)和[事件与 API](diagram/events.md)。

## 另请参阅

- [交互式编辑器](diagram/editor.md)
- [事件与 API](diagram/events.md)
- [JavaScript 图表](charts.md)
- [Designer](../../designer.md) —— 桌面版可视化策略编辑器
- [JS-Diagram 代码仓库](https://github.com/StockSharp/JS-Diagram)
