# 优化曲面

`SurfaceWidget` 把参数搜索的结果显示为三维地形：一个指标位于两条离散轴之上，指标的数值同时决定高度和颜色。该控件接收与[优化热力图](optimization_heatmap.md)相同的数据，因此同一组结果既可以用平面图显示，也可以用曲面显示，或者两者同时显示。

![优化曲面：把结果表现为按两个参数展开的地形](../../../../images/javascript_controls_optimization_surface.png)

## 创建和更新

```ts
import {
  HeatDirections,
  SurfaceWidget,
  type SurfaceData,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const surface = SurfaceWidget.create(
  document.querySelector<HTMLElement>('#surface')!,
  {},
  { host },
);

const sweep: SurfaceData = {
  xLabel: 'Fast',
  yLabel: 'Slow',
  metricLabel: 'Net profit',
  betterWhen: HeatDirections.Higher,
  cells: [
    { x: '10', y: '50', value: 1_250 },
    { x: '10', y: '80', value: -320 },
    { x: '20', y: '50', value: 2_480 },
    { x: '20', y: '80', value: 640 },
  ],
};

surface.update(sweep);
```

唯一的依赖是 `host`；接口 `SurfaceDeps` 不包含其他字段。`create` 的第二个参数是实例的已保存状态：曲面既不读取它，也不向其中写入任何内容。

`update` 会整体替换整组数据。曲面代表一次完整的参数搜索，因此它没有部分更新：把一次搜索的一半叠在另一次搜索的一半之上，得到的会是由两份不同汇总拼成的地形。

静态属性 `SurfaceWidget.TYPE` 的值为 `ControlTypes.OptimizationSurface`，即标识符 `optimizationSurface`。

## 数据

`HeatCell` 单元格是一对轴取值和一个测量到的指标：`{ x, y, value }`。轴取值是字符串，因为轴是离散的：`10`、`00:05:00` 和 `True` 都是轴上地位平等的位置。数值型取值按数字排序，其余按文本排序，因此 5、8、12、40 的搜索仍然保持为一个有序序列。

`betterWhen` 字段是必填的，接受 `HeatDirections.Higher` 或 `HeatDirections.Lower`。没有它，回撤的地形会把最差的角落抬成顶峰。

同一参数对上的多次运行会合并成平均值——这就是一个单元格。搜索没有走过的参数对留作空洞：只有在一个面的四个角都已知时才绘制该面，而空缺不会被插值——缺失的结果不等于零结果。如果完全没有单元格，或者至少一条轴上不同取值少于两个，就用按 `NoOptimizationResults` 键取得的文本作为占位内容代替地形。

`xLabel`、`yLabel` 和 `metricLabel` 用作轴的标注；`metricLabel` 还会额外显示在面板标题中。

## 显示

面的高度是数值相对于色阶基准点的位置，被折算到从底面到顶部的区间内：基准点落在高度的中间，因此底面并不表示“最差结果”，而是色阶的下边界。颜色取自 `host.presentation.canvasPalette()`：优于基准点的数值用 `up`，劣于基准点的用 `down`，离基准越远饱和度越高。各个面从远到近依次填充，因此靠前的山脊会遮住它后面的东西，并用 `grid` 颜色描边，以便在两个相邻面颜色几乎相同的地方仍能看清网格。

投影是正交的：人们通过在整个区域上比较高度来阅读曲面，而透视投影会让山脊的远端相对于近端被缩短。

地形下方绘制两条底边和一条垂直的刻度轴。参数轴上最多显示八个刻度：只要放得下就全部显示，再往后就每隔一个、每隔两个，以此类推，而且第一个和最后一个始终会被标注。垂直轴上有五个刻度，用指标数值标注。标注会被移到更靠近观察者的那两条底边上——旋转时会重新计算，以免数字压在网格之上。

## 视角与手势

所有手势都通过 pointer 事件处理，因此鼠标、触控笔和手指走的是同一条路径：

- 用一个指针拖动可旋转曲面：水平方向改变 `yaw`，垂直方向改变 `pitch`；
- 两个指针通过它们之间的距离改变缩放；旋转此时仍由单个指针负责；
- 滚轮同样改变缩放。无论浏览器以像素、行还是页为单位报告增量，它都会被换算成“档”，并且每个事件最多两档，使鼠标和触控板给出的步长相当。

画布之上的手势由控件独占，否则在手机上的拖动和桌面浏览器上的滚轮就会滚动页面而不是地形。

倾斜和缩放都有限制：`pitch` 从 `MIN_PITCH`（0.12）到 `MAX_PITCH`（1.45），缩放从 0.4 到 4。倾斜为零时每个面都会退化成一条线，而成直角时曲面就变成了平面图，也就是另一个控件了。`yaw` 的旋转不受限制，而是首尾相接：把地形转过一整圈以查看山脊背面的斜坡，是一个有意义的手势。初始视角是 `DEFAULT_VIEW`；面板标题栏中的按钮（`ResetView`）可以回到它。

当指针不在旋转曲面时，控件会在 22 CSS 像素的半径内寻找最近的已测量点。找到的点用 `up` 颜色画上圆环，而画布上方的条带中出现一行内容：`xLabel` 轴上的取值、`yLabel` 轴上的取值和指标。这个条带覆盖在画布之上，而不在面板标题栏中：读数属于指针下方的那个点，就应该待在它旁边。搜索没有走过的参数对不会提供给指针；距离相等时选择更靠近观察者的那个点。

## 控件做什么，宿主做什么

控件自行处理手势，通过 `ResizeObserver` 跟踪画布尺寸，并按当前尺寸和屏幕像素密度重绘地形，负责视角重置按钮和调用 `host.close()` 的面板关闭按钮，并通过 `host.register` 注册、在 `dispose` 中注销。画布的颜色和字体来自 `host.presentation.canvasPalette()`。全部可见文本都通过 `host.t` 请求：`OptimizationSurface`、`ResetView`、`ClosePanel`、`OptimizationSurfaceChart`、`NoOptimizationResults`。

数据由宿主提供：控件不启动优化，不订阅其进展，也不知道结果是怎么得到的——传给 `update` 的是什么，画出来的就是什么。面上没有点击操作：一个面对应一个单元格，而不是某一次运行，因此点它没有什么可打开的——按下即旋转地形。

控件不在 `host.preferences` 中保存自己的设置，也没有对应的键，并且不调用 `host.persistState`。当前视角可通过 `view()` 方法获取——如果需要在会话之间恢复它，由宿主自行保存并回传这些数值。

## 公共方法

- `update(data)` — 整体显示一组结果。
- `view()` — 返回当前视角的副本（`yaw`、`pitch`、`zoom`）。
- `resetView()` — 把视角恢复为 `DEFAULT_VIEW`。
- `dispose()` — 断开尺寸观察器、调用 `host.unregister` 并移除根元素。

## 辅助函数

几何计算已从控件中抽出到单独的模块，并由本包导出——可以在它之上构建自己的绘制：

- `surfaceLayout(input)` — 针对给定的尺寸和视角，把单元格分解为面、轴和顶点；没有可画内容时返回 `null`。
- `project(nx, ny, nz, view, box)` — 把单位立方体中的一个点投影到画布上。
- `dragView(view, dx, dy)` — 拖动若干像素之后的视角。
- `zoomView(view, factor)` — 改变缩放之后的视角。
- `clampView(view)` — 被约束到允许范围内的视角。
- `DEFAULT_VIEW`、`MIN_PITCH`、`MAX_PITCH` — 初始视角和倾斜的边界。

## 另请参阅

- [JavaScript 交易控件](../trading_controls.md)
- [优化热力图](optimization_heatmap.md)
- [统计](statistics.md)
- [权益曲线](equity.md)
