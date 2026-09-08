# 策略

`StrategiesWidget` 显示正在运行的策略列表：每个策略一行，包含其状态、交易模式、持仓、订单和成交计数、盈利以及控制按钮。控件标识符为 `strategies`（`ControlTypes.Strategies`），也可通过静态属性 `StrategiesWidget.TYPE` 获取。

![带有状态、持仓、PnL 和收益曲线的策略列表](../../../../images/javascript_controls_strategies.png)

## 创建和更新

```ts
import {
  StrategiesWidget,
  StrategyStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let strategies!: StrategiesWidget;

strategies = StrategiesWidget.create(
  document.querySelector<HTMLElement>('#strategies')!,
  {},
  {
    host,
    tradingModes: ['Disabled', 'CancelOrders', 'ReducePosition', 'Full'],
    start: id => console.log('start', id),
    stop: id => console.log('stop', id),
    closePosition: id => console.log('flatten', id),
    openStrategy: id => console.log('open', id),
    riskRules: id => console.log('risk', id),
    setTradingMode: (id, mode) => console.log('mode', id, mode),
  },
);

strategies.update([{
  id: 'sma-1',
  name: 'SMA crossover',
  state: StrategyStates.Started,
  online: true,
  tradingMode: 'Full',
  portfolio: 'Demo',
  security: 'BTC@IMEX',
  position: 0.25,
  ordersCount: 12,
  tradesCount: 8,
  pnlChange: 105,
  realized: 20,
  unrealized: 85,
  pnl: [
    { time: 1, value: 0 },
    { time: 2, value: 60 },
    { time: 3, value: 105 },
  ],
}]);
```

`update` 接收完整的数据行集合并用它替换表格：不在所传列表中的策略被视为已删除，其数据行会消失。控件没有单行的流式更新——新状态以整个列表的形式到达。

`create` 的第二个参数是实例的已保存状态。控件既不读取它，自己也不保存任何东西：既不写 `host.preferences` 中的键，也不调用 `host.persistState`。

`StrategyStates` 对象导出 `Stopped`、`Starting`、`Started` 和 `Stopping` 状态。

## 依赖

只有宿主是必填的。宿主的完整性在创建时由 `assertHost` 函数检查，因此不完整的 `TradingHost` 会导致抛出异常并指出缺失成员的名称，而不是留下一个不起作用的按钮。

| 依赖 | 是否必填 | 默认行为 |
|---|---|---|
| `host` | 必填 | — |
| `start(id)` | 可选 | 不创建启动按钮。 |
| `stop(id)` | 可选 | 不创建停止按钮。 |
| `closePosition(id)` | 可选 | 持仓列中只保留数字。 |
| `openStrategy(id)` | 可选 | 不创建跳转到策略的按钮。 |
| `riskRules(id)` | 可选 | 不创建风险规则按钮。 |
| `setTradingMode(id, mode)` | 可选 | 交易模式以文本形式显示。 |
| `tradingModes` | 可选 | 空列表，不创建模式下拉框。 |

这样的组合允许搭建一个只读面板：如果不传入任何操作函数，表格就只显示数据，而不显示任何按钮。

`tradingModes` 中的字符串会经过 `host.t`，也就是说它们是翻译键。它们属于宿主而不是本包，因此它们不出现在 `translation-keys.json` 中是正常的，翻译由宿主完成。

## 状态与操作

状态单元格由一个圆点和一个词组成：圆点便于快速浏览列表，词则区分 `Starting` 和 `Started`。如果行中填写了 `error` 字段，错误文本会同时出现在圆点和词的工具提示中，而因故障停止的策略会用“错误”而不是“已停止”标注。

行内按钮只为宿主传入的函数创建，并且只在状态允许的地方启用：

- 启动 — 仅对处于 `Stopped` 状态的策略；
- 停止 — 仅对处于 `Started` 状态的策略；
- 平仓 — 仅对持仓不为零的运行中策略；
- 风险规则和跳转到策略 — 始终可用。

交易模式下拉框只在策略已停止时可用：模式决定策略将以什么方式启动，而不是交易过程中的一个操纵杆。修改模式会调用 `setTradingMode`；控件自身不会改变行中的值，而是等待下一次 `update`。

## 列与外观

表格中显示状态、操作、在线标志、交易模式、名称、投资组合、标的、持仓、订单数和成交数、盈利变化、盈利图表、已实现和未实现盈利、错误。在线标志是综合性的：只有当策略既已构建又已连接时才算在线，这由数据提供方决定。

持仓、盈利变化以及两个盈利数值的颜色类由 `host.presentation.pnlClass` 返回。盈利变化还会额外标注方向箭头；变化为零时没有箭头。

图表列在 140 × 26 CSS 像素的区域内，按 `pnl` 中的点绘制累计盈利曲线。画布创建时考虑了 `devicePixelRatio`，因此在高像素密度屏幕上线条依然锐利。颜色取自 `host.presentation.canvasPalette()`，曲线按运行的最终结果着色：一个曾冲到高点又把一切回吐的策略会显示为亏损。没有 `pnl` 点时单元格保持为空。

默认排序是按名称升序：人们自上而下阅读列表来寻找某个具体策略，而随盈利变动重排行会妨碍这种阅读。面板还支持多行选择、上下文菜单、筛选和导出为 XLSX。

## 留给宿主的部分

控件不会启动或停止策略，不发送订单，也不平仓——它只调用传入的函数，然后等待新的数据行列表。

`pnlChange` 的值控件按原样接收：起算点由数据提供方选择。这样，重启后的策略就不会继续从重启之前的时刻开始计算变化。

面板的关闭按钮调用 `host.close()`，导出会生成 `strategies` 文件。实例在创建时向宿主注册，并在 `dispose` 中注销。

## 公共方法

- `StrategiesWidget.create(hostEl, state, deps)` — 构建面板并把它添加到容器中。
- `update(rows)` — 替换整个策略列表。
- `dispose()` — 注销、删除表格并释放资源。

## 另请参阅

- [JavaScript 交易控件](../trading_controls.md)
- [持仓](positions.md)
- [活动订单](active_orders.md)
- [成交历史](trade_history.md)
