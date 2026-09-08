# 优化热力图

`OptimizationHeatmapWidget` 按两个参数绘制一个指标：第一个参数的取值沿水平方向排列，第二个沿垂直方向排列，交点处是一个单元格，其颜色代表这组参数所取得的结果。控件对优化过程本身一无所知——不论这些参数对是怎么算出来的，指标在两个轴上的分布图都是一样的——因此参数对由宿主连同轴名和指标名一起传入。

![按两个参数绘制的优化热力图](../../../../images/javascript_controls_optimization_heatmap.png)

## 创建和更新

```ts
import {
  HeatDirections,
  OptimizationHeatmapWidget,
  type HeatCell,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const heatmap = OptimizationHeatmapWidget.create(
  document.querySelector<HTMLElement>('#heatmap')!,
  {},
  { host },
);

const cells: HeatCell[] = [
  { x: '10', y: '00:05:00', value: 12_400 },
  { x: '10', y: '00:15:00', value: 9_150 },
  { x: '20', y: '00:05:00', value: -1_800 },
  { x: '20', y: '00:15:00', value: 15_900 },
];

heatmap.update({
  xLabel: 'Length',
  yLabel: 'Timeframe',
  metricLabel: 'Net profit',
  betterWhen: HeatDirections.Higher,
  cells,
});
```

唯一的依赖是 `host`；接口 `OptimizationHeatmapDeps` 不包含其他字段。热力图不接受处理器：一个单元格是同一参数对多次运行的平均值，而不是某一次运行，因此点击时没有什么可打开的。`create` 的第二个参数是实例的已保存状态；控件既不读取它，也不向其中写入任何内容。

`update` 会整体替换整张图：从数据集合中退出的参数对不再存在，为它保留下来的单元格会指向一次报告中已经不存在的运行。

`update` 的参数是 `HeatmapData` 对象，包含以下字段：

| 字段 | 用途 |
|---|---|
| `xLabel` | 水平轴的名称，标注在图的下方。 |
| `yLabel` | 垂直轴的名称，标注在图的上方。 |
| `metricLabel` | 指标名称，显示在面板标题中。 |
| `betterWhen` | 哪个方向更好：`HeatDirections.Higher`（`'higher'`）或 `HeatDirections.Lower`（`'lower'`）。该字段是必填的——没有它，回撤图会把最差的角落涂成表示胜利的颜色。 |
| `cells` | `HeatCell` 测量结果：`x` 和 `y` 是以字符串表示的轴取值，`value` 是数字。 |

`HeatmapData` 类型声明在控件所在的模块中，而包的根导出并不重新导出它：需要显式标注类型时，请从子路径 `@stocksharp/trading-controls/optimization-heatmap-widget` 导入。

静态属性 `OptimizationHeatmapWidget.TYPE` 的值为 `ControlTypes.OptimizationHeatmap`，即标识符 `optimizationHeatmap`。

## 数据与显示

坐标轴是离散的，因此它们的取值以字符串传入：`10`、`00:05:00` 和 `True` 都是轴上地位平等的位置。当所有取值都是数字时按数值排序（否则 `10` 会排在 `2` 前面，图的形状就成了数字写法的产物），其余情况按文本排序；对于 .NET 写出的定宽时间间隔字符串，文本顺序与时间顺序一致。网格的第一行位于图的底部：这是一张图表，Y 轴向上增长。

同一参数对上的多次运行会合并为一个单元格——取平均值并附带运行次数；指标为非数字的记录会被丢弃，而不会毁掉整个单元格。颜色相对于一个基准值计算：如果测量结果跨越了零，基准值就取零，否则取取值区间的中点，因为把基准锚定在一个搜索根本没有到达的零上，只会得到一片没有对比度的均匀色块。基准两侧到达满色的跨度是相同的，因此各处相同的饱和度就意味着指标相同的偏离幅度。`betterWhen` 的方向体现在符号中：最好的结果始终用增长色绘制。

单元格中不打印数字：在四十乘四十的搜索中，格子里的数字根本无法辨认，而读取颜色正是这张图的意义所在。没有被运行过的参数对不会留空，而是画上一道对角斜线：未测试的参数对与结果为零的参数对是两回事，而以零为中性点的色阶会把它们画成一样。最好的单元格用网格颜色描边——那是调色板中唯一没有方向含义的颜色。

图的上方绘制由这两种颜色构成的图例，并带三个标注：下边界、基准值和上边界。轴标签在放不下时会被抽稀，但轴的端点值始终会被标注。数字通过 `formatStatistic` 输出——与统计面板使用的格式相同：四舍五入到两位小数。

鼠标悬停在已测量的单元格上时会出现提示，显示两个轴的取值和指标。只有在平均了不止一次运行时，提示中才会加入运行次数；而 `Best` 标记只出现在最好的单元格上。画有斜线的单元格上没有提示：它已经明显表示为未测试。提示会贴靠画布边界，以免在边缘单元格处跑出画面之外。

只要还没有任何测量结果，就用按 `NoOptimizationResults` 键取得的文本作为占位内容代替热力图。

## 控件做什么，宿主做什么

控件自行构建面板的布局，通过 `ResizeObserver` 让画布适配容器，并按 `devicePixelRatio` 建立物理像素的缓冲区，否则热力图画出的网格会是发丝般的细线。每次绘制时它都向 `host.presentation.canvasPalette()` 请求颜色和字体：`up` 和 `down` 是色阶的两端，`grid` 用于网格、斜线、最佳单元格的边框和标注，`font` 是画布上文字的字体。本包在这里不选择自己的调色板，因此宿主切换主题时热力图会以新的颜色重绘。透明度是热力图自己唯一支配的东西。

面板的关闭按钮调用 `host.close()`，实例通过 `host.register` 注册，并在 `dispose` 中注销。全部可见文本都通过 `host.t` 请求：`OptimizationHeatmap`、`OptimizationHeatmapChart`、`ClosePanel`、`NoOptimizationResults`、`Runs`、`Best`。控件不在 `host.preferences` 中保存自己的设置，也没有对应的键。

数据由宿主提供：热力图不发起参数搜索，不选择也不计算指标，更不去猜测“越大越好”还是“越小越好”——传给 `update` 的是什么，画出来的就是什么。

## 公共方法

- `update(data)` — 整体显示热力图。
- `dispose()` — 断开尺寸观察器、调用 `host.unregister` 并移除根元素。

## 辅助函数

热力图的全部几何计算都被抽到单独的模块中并由本包导出——不用控件也可以使用它们：

- `layoutHeatmap(input)` — 热力图的布局：网格、空缺、标注、图例和色阶；没有测量结果时返回 `null`。
- `hitHeatmap(layout, x, y)` — 指定点下的单元格，没有则返回 `null`。
- `heatScale(buckets)` — 基准值、跨度和区间边界。
- `tintOf(value, scale, betterWhen)` — 从 −1 到 1 的饱和度，其中正值始终表示更好。
- `valueAt(tint, scale, betterWhen)` — 反向变换，用于图例的标注。
- `HeatDirections` — 方向 `Higher` 和 `Lower`。

## 另请参阅

- [JavaScript 交易控件](../trading_controls.md)
- [优化曲面](optimization_surface.md)
- [统计](statistics.md)
- [权益曲线](equity.md)
