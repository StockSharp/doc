# 权益曲线

`EquityWidget` 把一次运行的累计 P&L 绘制成随时间变化的图表。该面板以标识符 `equity`（`ControlTypes.Equity`）注册，曲线本身由 `@stocksharp/chart` 引擎绘制，该包作为对等依赖引入。

![按运行结果绘制的权益曲线](../../../../images/javascript_controls_equity.png)

## 创建和更新

```ts
import {
  EquityWidget,
  type PnlPoint,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const equity = EquityWidget.create(
  document.querySelector<HTMLElement>('#equity')!,
  {},
  { host },
);

const start = Date.now() - 3 * 60 * 60 * 1000;

const points: PnlPoint[] = [
  { time: start, value: 0 },
  { time: start + 45 * 60 * 1000, value: 320.5 },
  { time: start + 95 * 60 * 1000, value: -140.25 },
  { time: Date.now(), value: 1_180.75 },
];

equity.update(points);
```

`EquityDeps` 的唯一依赖就是 `host`；面板没有任何操作处理器，因为曲线上没有什么可执行的。`create` 的第二个参数——实例的已保存状态——控件并不使用：面板既不在状态中、也不在 `host.preferences` 中保存自己的设置。

`update` 会整体替换一次运行。曲线的值是累计的，因此一组不包含此前所传采样的点被视为另一次运行，而不是原有运行的延续。

## 数据与绘制

`PnlPoint` 中的 `time` 字段是 unix 毫秒；引擎以秒计时，转换由控件自己完成。点按时间排序，`time` 或 `value` 为非数字的采样会被丢弃，同一秒内的多个数值只保留最后一个——它才代表该时刻运行的真实位置。

只要可用的点少于两个，面板就显示按 `NoEquity` 键取得的消息，不创建图表，且 `chart()` 返回 `null`。图表本身在第一次绘制时创建，而不是在构造函数中：引擎需要一个已经放置在页面上的容器。区域尺寸的变化由 `ResizeObserver` 跟踪，它会调用引擎的 `resize`。

颜色、字体和网格颜色取自 `host.presentation.canvasPalette()`。曲线按运行的最后一个数值着色：数值不小于零时用 `up`，为负时用 `down`，线下方的填充使用同一颜色，并从上到下由 28 % 渐弱至 2 %。时间轴按浏览器时区显示小时和秒，`Intl` 不可用时则使用 UTC。

## 标题、提示和按钮

面板顶部以 `formatPnl` 格式显示本次运行的最后一个数值——带符号并保留两位小数；颜色类由 `host.presentation.pnlClass` 返回。曲线上方显示指针所指处的数值：用 `host.presentation.timeText` 表述的时刻，以及紧随其后的数值本身。指针离开图表时提示被清空。

标题栏中提示为 `ResetView` 的按钮调用 `resetZoom`，关闭按钮调用 `host.close()`。面板通过 `Equity`、`ResetView`、`ClosePanel`、`PnLChart` 和 `NoEquity` 这些键获取可见文本。

控件自己负责曲线的形状、颜色和缩放。留给宿主的是文字的语言、调色板、时刻的格式以及点本身的来源：控件不计算 P&L，也不向任何地方请求数据。

## 公共方法

- `EquityWidget.create(hostEl, state, deps)` — 创建面板并把它的根元素添加到容器中。
- `EquityWidget.TYPE` — 标识符 `equity`。
- `update(points)` — 整体显示一次运行。
- `resetZoom()` — 缩放之后恢复到整次运行的全景视图。
- `chart()` — `IChartApi` 实例，供宿主补画自己的内容：基准线、回撤标记。图表出现之前返回 `null`。
- `dispose()` — 断开尺寸观察器、删除图表并在宿主中注销。

属性 `rootEl` 给出面板的根元素。

## 引入图表引擎

`@stocksharp/chart` 包需要单独安装：

```bash
npm install @stocksharp/chart
```

现成的 `sstradingcontrols.js` 捆绑包并不包含该引擎，因此在没有打包器的页面上要把它的脚本一并引入：

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/indicators/dist/ssindicators.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/chart/dist/sschart.js"></script>
```

## 另请参阅

- [JavaScript 交易控件](../trading_controls.md)
- [持仓](positions.md)
- [成交历史](trade_history.md)
- [JavaScript 图表](../charts.md)
