# 事件与 API

图表组件在界面外观方面刻意保持无头（headless）：它发出携带数据的事件，让宿主渲染菜单和对话框，然后暴露方法来驱动模型。正因如此，属性面板或上下文菜单是*你自己*的 UI，通过组件的事件连接起来。

## 事件

使用 `diagram.on(event, handler)` 订阅；它返回一个取消订阅的函数。

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — 节点生命周期。
- `linkAdded` / `linkRemoved` / `linkRelinked` — 连线生命周期。
- `linkValidation` — 每次尝试连接时返回 `{ allowed, reason }`。
- `selectionChanged` / `nodeSelected` / `linkSelected` — 选择状态。
- `contextMenuRequested` — 右键点击时返回 `{ x, y, node, link, port, commands }`。
- `fullscreenRequested` — `{ fullscreen }`；由宿主应用布局。

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('已连接', links[0]));
// 稍后：
off();
```

## 上下文菜单

组件报告点击位置以及已启用命令的列表；由你绘制弹出菜单并运行所选命令：

```js
diagram.on('contextMenuRequested', ({ x, y, commands }) => {
  // commands: { command, enabled }[]，其中 command 为以下之一：
  // undo | redo | cut | copy | paste | open | delete | properties | help
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);
  menu.onPick = command => diagram.executeContextCommand(command);
});
```

## 连线校验

端口是带类型的，组件会拒绝不兼容或超额订阅的连线，并发出 `linkValidation`，附带 `reason`（`incompatible-type`、`duplicate-link`、`source-limit`、`target-limit`、`same-node`……）。使用 `setLinkValidator` 添加你自己的规则：

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## 保存与加载

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // 带版本的文档
diagram.loadDocument(document);
```

## 撤销、重做与剪贴板

使用 `diagram.undo()` / `redo()`，并用 `canUndo()` / `canRedo()` 来控制按钮的可用状态；`copySelection()` / `cutSelection()` / `pasteSelection()` 和 `deleteSelection()` 用于剪贴板操作。`setReadOnly(true)` 将图表锁定为预览状态。

撤销/重做的可用性由控件掌控，因此请跟踪其规范的 `undoStackChanged` 事件，以便针对*每一个*命令（删除、拖拽、重新连线、粘贴）保持按钮同步，而不仅仅是上面提到的模型变更事件：

```js
diagram.on('undoStackChanged', ({ canUndo, canRedo }) => {
  undoButton.disabled = !canUndo;
  redoButton.disabled = !canRedo;
});
```

## 运行时状态与错误高亮

图表可以在方案之上叠加执行状态。`setNodeError` 会以动画脉冲（约 1 秒）闪烁节点的边框，并用红色高亮标记它 —— 用它来报告运行时故障。[交互式编辑器](editor.md)中的 **Error** 按钮正是这么做的。

```js
diagram.setNodeError('sma', 'SMA 失败：未配置数据源。');
diagram.setNodeError('sma', '警告', { animate: false }); // 标记它，但跳过初始闪烁
```

在加载时就已存在的错误会绘制成红色背景，而非闪烁 —— 将它们传给 `load`：

```js
diagram.load(nodes, links, { nodeErrors: { sma: '保存的周期值无效。' } });
```

其他运行时钩子：`setActiveNode(id)` 高亮当前正在执行的节点（调试器光标），`setPortRuntimeState(id, direction, portId, patch)` 标注单个端口，`setGlobalError(message)` 闪烁整个方案范围的错误。使用 `clearRuntimeState()` 清除所有内容。

## 参见

- [JavaScript 框图](../diagram.md)
- [交互式编辑器](editor.md)
