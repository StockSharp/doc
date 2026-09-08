# 波动率微笑

`OptionSmileWidget` 绘制某一期权系列的波动率微笑：看涨和看跌的隐含波动率按行权价展开，两条线共用一把标尺。图表由 `@stocksharp/chart` 包中的引擎绘制，该包声明为对等依赖。

![按期权链行权价展开的波动率微笑](../../../../images/javascript_controls_option_smile.png)

## 创建和更新

```ts
import {
  OptionSmileWidget,
  type OptionStrike,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const smile = OptionSmileWidget.create(
  document.querySelector<HTMLElement>('#smile')!,
  {},
  { host },
);

const chain: OptionStrike[] = [
  { strike: 67_000, call: { ivLast: 0.52 }, put: { ivBid: 0.55, ivAsk: 0.57 } },
  { strike: 67_500, call: { ivLast: 0.48 }, put: { ivLast: 0.50 } },
  { strike: 68_000, call: { ivBid: 0.45, ivAsk: 0.47 }, put: { ivLast: 0.47 } },
  { strike: 69_000, call: { ivLast: 0.49 }, put: {} },
];

smile.update(chain, { assetPrice: 68_120 });
```

唯一的依赖是 `host`；接口 `OptionSmileDeps` 不包含其他字段。`create` 的第二个参数是实例的已保存状态，微笑图既不读取它，也不向其中写入任何内容。

`update` 会整体替换整条链。第二个参数 `context` 是可选的，默认为空对象；它使用与期权 T 型报价表相同的 `OptionChainContext` 类型，但微笑图只需要其中的标的资产价格 `assetPrice`。

静态属性 `OptionSmileWidget.TYPE` 的值为 `ControlTypes.OptionSmile`，即标识符 `optionSmile`。

## 数据与显示

行权价按升序排序，`strike` 为非数字的记录会被丢弃。某一侧的波动率取自 `ivLast`，如果没有成交则取 `ivBid` 和 `ivAsk` 的平均值；只考虑有限的正值。如果两者都没有，就不绘制该点：行权价仍留在坐标轴上，而线条断开——这样便能看出只有一侧被报价的行权价。图上的数值以百分比输出。

行权价轴工作在 `ordinal` 模式：步距按挂牌清单均匀分布，而不是按数字之间的距离，因此 67_500 和 68_000 之间的跳跃不会变成一个空洞。坐标轴标签和十字线标记由同一个价格格式化器生成。

标的资产价格不以线条绘制——在序数轴上没有对应的刻度位置——而是在图例中与 `Call` 和 `Put` 两个键旁边以文本形式显示。鼠标悬停在图上时会出现一行内容，显示行权价及两侧在该处的数值：微笑图是通过两条曲线之间的距离来阅读的，因此两者都要显示。

只要还没有任何行权价被报价，就用按 `NoOptions` 键取得的文本作为占位内容代替图表。

## 控件做什么，宿主做什么

控件在收到首批数据时自行创建图表，从 `host.presentation.canvasPalette()` 获取颜色、字体和网格颜色（`up` 表示看涨，`down` 表示看跌），通过 `ResizeObserver` 跟踪容器尺寸并调整画布，处理重置缩放按钮以及调用 `host.close()` 的面板关闭按钮。全部可见文本都通过 `host.t` 请求：`OptionSmile`、`ResetView`、`ClosePanel`、`ImpliedVolatility`、`Call`、`Put`、`OptionChain`、`NoOptions`、`Underlying`。

数据由宿主提供：微笑图不订阅行情，不计算波动率，也不区分不同的系列——传给 `update` 的是什么，画出来的就是什么。控件不在 `host.preferences` 中保存自己的设置，也没有对应的键。

## 公共方法

- `update(strikes, context)` — 显示期权链及其被采集时的上下文。
- `resetZoom()` — 缩放之后恢复到整条链的全景视图。
- `chart()` — 返回图表对象（`IChartApi`），若图表尚未创建则返回 `null`；供需要在同一画布上添加第二条序列或标记的宿主使用。
- `dispose()` — 断开尺寸观察器、删除图表、调用 `host.unregister` 并移除根元素。

## 辅助函数

本包同时导出了绘制所依赖的那些函数——它们可以单独使用：

- `sideVolatility(side)` — 某一侧的波动率，未知时返回 `null`。
- `sortedChain(strikes)` — 按绘制顺序排列的期权链：按行权价升序，并剔除不合法的记录。
- `toSmileSeries(chain, put)` — 把一侧转换为图表所需的点集。

## 另请参阅

- [JavaScript 交易控件](../trading_controls.md)
- [期权 T 型报价表](option_desk.md)
- [权益曲线](equity.md)
- [订单簿](order_book.md)
