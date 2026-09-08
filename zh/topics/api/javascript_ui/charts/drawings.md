# 标注工具

`DrawingController` 是图表的手工标注层：线条、图形、斐波那契水平线和持仓草图。控制器把图形保存为纯 JSON 对象，把它们绑定到画布原语，让每一次修改都走图表的撤销栈，并负责用鼠标逐步构建图形。

## 引入

该层作为 `@stocksharp/chart` 包的一个独立入口点提供：

```ts
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';
```

导入该入口点会立刻把所有内置的标注类型注册到公共目录 `drawingDefinitionRegistry` 中。

## 创建和更新

控制器只需要图表；命令栈默认也从图表获取（`chart.commandStack()`），因此撤销和重做与图表上的其他操作一起工作：

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';

const chart = createChart(document.getElementById('chart')!, { timeScale: { timeVisible: true } });
chart.addSeries(CandlestickSeries, {}).setData(candles);

const drawings = new DrawingController({ chart });

// 水平价位线：一个点，时间为以秒为单位的 Unix 时间。
const level = drawings.create(
  BuiltInDrawingType.HorizontalLine,
  [{ time: 1_712_000_000, price: 68_000 }],
  { options: { color: '#f5c542', lineWidth: 2 } },
);

// 位于子面板上的两点趋势线通过 paneId 指定。
drawings.create(
  BuiltInDrawingType.TrendLine,
  [
    { time: 1_712_000_000, price: 67_400 },
    { time: 1_712_600_000, price: 69_150 },
  ],
  { paneId: 'main' },
);

drawings.updateOptions(level.id, { lineWidth: 3 });
drawings.setLocked(level.id, true);

// 集合的快照按 zOrder 排序后到达。
drawings.subscribe(items => console.log(items.length));

chart.commandStack().undo();
```

`create` 会补齐缺失的字段：`paneId` 默认为 `main`，`visible` 为 `true`，`locked` 为 `false`，`zOrder` 比当前最大值大一，而选项则叠加在该类型的 `defaultOptions` 之上。`add` 整体插入一个现成的实例，`duplicate` 复制已有实例，`remove` 和 `clear` 负责删除。上述每一次调用都恰好向历史中放入一条可撤销的命令。

`update` 可以修改字段的任意组合（`points`、`options`、`paneId`、`visible`、`locked`、`zOrder`）；`updateOptions`、`setVisible`、`setLocked` 和 `moveToPane` 是常见场景的简写形式。写入之前实例会被规范化：点和选项会检查 JSON 兼容性并被冻结，点的数量会与类型的定义比对，面板则与图表中已有的面板比对。

## 内置类型

标识符集中在 `BuiltInDrawingType` 中；其字符串值就是所保存图形的 `type` 字段。

| 常量 | 值 | 点数 | 选项 |
|---|---|---|---|
| `HorizontalLine` | `horizontal-line` | 1 | `LineDrawingOptions` |
| `VerticalLine` | `vertical-line` | 1 | `LineDrawingOptions` |
| `TrendLine` | `trend-line` | 2 | `LineDrawingOptions` |
| `Ray` | `ray` | 2 | `LineDrawingOptions` |
| `Rectangle` | `rectangle` | 2 | `RectangleDrawingOptions` |
| `Text` | `text` | 1 | `TextDrawingOptions` |
| `Note` | `note` | 1 | `TextDrawingOptions` |
| `FibonacciRetracement` | `fibonacci-retracement` | 2 | `FibonacciDrawingOptions` |
| `Measure` | `measure` | 2 | `MeasureDrawingOptions` |
| `LongPosition` | `long-position` | 3 | `PositionDrawingOptions` |
| `ShortPosition` | `short-position` | 3 | `PositionDrawingOptions` |

各套选项因用途而异：

- `LineDrawingOptions` — `color`、`lineWidth`（取值范围 (0, 20]）、`lineStyle`（0…4）。
- `RectangleDrawingOptions` — 与上面相同，另加用于填充的 `fillColor`。
- `TextDrawingOptions` — `text`（最多 10 000 个字符，换行符有效）、`color`、`backgroundColor`、`borderColor`、`borderWidth`、`fontSize`、`fontFamily`、`padding`。`Note` 与 `Text` 的区别仅在默认值：底衬、边框和更大的内边距。
- `FibonacciDrawingOptions` — `levels`（2 至 32 个取值，范围为 [-5, 5]；重复项会被去除，列表会被排序）、`labelsVisible`、`extendRight`，以及 `color`、`lineWidth`、`lineStyle`、`fillColor`、`fontSize`。
- `MeasureDrawingOptions` — `color`、`lineWidth`、`fillColor`、`labelColor`、`labelBackgroundColor`、`fontSize`。标签显示所选区间的价格变化、百分比和持续时间。
- `PositionDrawingOptions` — `entryColor`、`targetColor`、`stopColor`、`targetFillColor`、`stopFillColor`、`textColor`、`lineWidth`、`fontSize` 和 `quantity`。三个点按顺序给出：入场、目标、止损；据此计算标签中的盈利、风险和 R:R 比值。

任何未通过类型检查的选项值都会导致抛出异常——无法保存线宽不合法或颜色为空的图形。

## 用鼠标构建

逐步输入由控制器自己完成：它把图表切换到绘制模式，并订阅点击和十字线。

```ts
drawings.subscribeCreation(state => {
  if (state === null) return;                    // 构建已完成或已取消
  console.log(state.name, state.points.length, state.minimumPoints, state.maximumPoints);
});

drawings.beginCreation(BuiltInDrawingType.Rectangle, { options: { color: '#26a69a' } });

// 在图形尚未取得所需点数之前，可按 Esc 取消。
document.addEventListener('keydown', event => {
  if (event.key === 'Escape') drawings.cancelCreation();
});
```

每次点击添加一个经过磁吸处理的点；光标移动会更新草稿，草稿用与成品图形相同的原语绘制，但不进入历史。面板在第一次点击时固定，其他面板中的点击会被忽略。一旦取得的点数达到该类型允许的最大值，构建就会自行结束并创建一个普通图形。`finishCreation` 提前结束构建，若点数少于最小值则返回 `null`；`cancelCreation` 丢弃草稿；`creation` 给出当前的 `DrawingCreationSnapshot` 快照。

## 吸附到柱体

磁吸会把点拉到当前面板中各序列的数值上——计算在屏幕坐标中进行，依据到候选点的垂直距离。

```ts
import { DrawingMagnetMode } from '@stocksharp/chart/drawings';

const drawings = new DrawingController({
  chart,
  magnet: { mode: DrawingMagnetMode.Weak, maxDistance: 12 },
});

drawings.applyMagnetOptions({ mode: DrawingMagnetMode.Strong });
console.log(drawings.magnetOptions());
```

`DrawingMagnetMode.None` 关闭吸附，`Weak`（默认模式）只在 `maxDistance` 范围内吸附——默认为 10 CSS 像素，`Strong` 则总是吸附到最近的数值。构建过程中修改设置会立即重新计算预览点。

## 保存与恢复

`DrawingInstance` 有意不包含任何运行时对象，因此整套标注可以按原样序列化：

```ts
import type { DrawingInstance } from '@stocksharp/chart/drawings';

const saved = JSON.stringify(drawings.drawings());

const result = drawings.replaceAll(JSON.parse(saved) as DrawingInstance[], { unknownType: 'skip' });
console.log(result.restored.length, result.skipped);
```

`replaceAll` 会整体替换全部标注：先校验所有输入实例（标识符重复即为错误），然后把旧图形从图表上移除，再添加新的。只要有一个图形没能加上，就恢复到之前的状态。在 `skip` 策略下（默认），未知的 `type` 会以 `unknown-type` 原因进入 `skipped`；在 `error` 策略下则会中断恢复。恢复会清空命令历史，因此不能在事务内部调用它。

## 自定义标注类型

类型目录是可扩展的。只需描述定义并返回到原语的绑定即可——带选中、控制点和拖动的现成外壳由 `createInteractiveDrawingBinding` 提供：

```ts
import { createInteractiveDrawingBinding, registerDrawing } from '@stocksharp/chart/drawings';

registerDrawing({
  type: 'price-band',
  name: 'Price Band',
  points: { min: 2, max: 2 },
  defaultOptions: { color: '#4a9eff' },
  normalizeOptions: options => Object.freeze({ color: String(options.color).trim() }),
  create: (instance, events) => createInteractiveDrawingBinding(instance, events, {
    draw(context) {
      const [first, second] = context.points;
      if (second === undefined) return;
      context.context.strokeStyle = String(context.instance.options.color);
      context.context.strokeRect(
        context.plot.x, Math.min(first.y, second.y),
        context.plot.width, Math.abs(second.y - first.y),
      );
    },
    hitTest(point, context) {
      const [first, second] = context.points;
      if (second === undefined) return null;
      return point.y >= Math.min(first.y, second.y) && point.y <= Math.max(first.y, second.y)
        ? { cursor: 'move' }
        : null;
    },
  }),
});

drawings.create('price-band', [
  { time: 1_712_000_000, price: 67_800 },
  { time: 1_712_600_000, price: 68_900 },
]);
```

`draw` 收到屏幕坐标点、绘图区域的矩形、主题、缩放系数和是否被选中的标志；`hitTest` 回答光标是否落在图形本体上。可选的 `autoscaleInfo` 和 `handleColor` 决定是否参与自动缩放以及控制点的颜色。`normalizeOptions` 在每次写入模型之前被调用——那是唯一适合校验选项值的地方。

拖动图形本体或单个点会经由事件 `preview`（中间状态，不写入历史）、`commit`（一条 “Edit drawing” 命令）和 `cancel`（回到手势之前的状态）。被锁定的图形（`locked`）既不能拖动，也不显示控制点。

目录也可以直接管理：`unregisterDrawing(type)`、`getDrawingDefinition(type)`、`getDrawingTypes()`，而 `DrawingDefinitionRegistry` 允许建立一个独立的目录，并通过 `registry` 参数传给控制器。

## 公共方法

`DrawingController`：

- `drawings()`、`get(id)`、`has(id)` — 读取当前集合。
- `create(type, points, options?)`、`add(instance)`、`duplicate(id, duplicateId?)` — 添加图形。
- `update(id, patch)`、`updateOptions(id, patch)`、`setVisible(id, visible)`、`setLocked(id, locked)`、`moveToPane(id, paneId)` — 修改。
- `remove(id)`、`clear()` — 删除。
- `beginCreation(type, options?)`、`finishCreation()`、`cancelCreation()`、`creation()` — 用鼠标构建。
- `magnetOptions()`、`applyMagnetOptions(patch)` — 吸附到柱体。
- `replaceAll(instances, options?)` — 恢复已保存的集合。
- `subscribe(listener)` / `unsubscribe(listener)`、`subscribeCreation(listener)` / `unsubscribeCreation(listener)` — 订阅。
- `dispose()` — 释放资源。

构造函数接受 `chart`（必填），以及 `registry`、`commandStack`、`idFactory` 和 `magnet`。

该入口点还导出该层的其余部分：用于自行计算吸附的 `DrawingMagnet`，`InteractiveDrawingPrimitive` 及 `createInteractiveDrawingBinding`，校验函数 `normalizeDrawingInstance` 和 `normalizeDrawingOptions`，现成的定义集合 `builtInLineDrawingDefinitions`、`builtInShapeDrawingDefinitions`、`builtInAnalysisDrawingDefinitions`、`builtInPositionDrawingDefinitions`，以及与之配对、用于注册到自有目录的函数 `registerBuiltInLineDrawings`、`registerBuiltInShapeDrawings`、`registerBuiltInAnalysisDrawings`、`registerBuiltInPositionDrawings`。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [K线图](candlestick.md)
- [指标](indicators.md)
- [历史数据回填](backfill.md)
