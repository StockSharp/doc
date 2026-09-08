# 从图表交易

`TradingLayer` 是与经纪商无关的图表交易状态层：它保存规范化后的订单、持仓、成交和报价，并把用户操作以意图（`TradingIntent`）的形式向外输出。该层自己不发送任何东西——其中既没有传输通道，也没有账户和重试，与经纪商的通信仍由宿主负责。

![图表之上的订单线、持仓和保护性订单](../../../../images/javascript_charts_trading.png)

## 引入

该层作为 [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) 包的一个独立入口点提供：

```ts
import { TradingLayer, TradingLayerPrimitive } from '@stocksharp/chart/trading';
```

不使用打包器时，同样的类可以在全局对象 `SSChart` 中取得（`new SSChart.TradingLayer({ tickSize: 0.25 })`）。

## 创建和更新

该层以价格网格参数创建，`TradingLayerPrimitive` 原语把它的状态绘制在序列之上，而 `TradingOrderPlacementAdapter` 把图表上的点击转换成下单意图：

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  TradingLayer,
  TradingLayerPrimitive,
  TradingOrderPlacementAdapter,
  type TradingIntent,
} from '@stocksharp/chart/trading';

declare const broker: { send(intent: TradingIntent): Promise<void> };

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {
  timeScale: { timeVisible: true },
});
const candles = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const layer = new TradingLayer({ tickSize: 0.25 });

chart.attachPrimitive(new TradingLayerPrimitive(layer, { showInactiveOrders: false }), {
  series: candles,
});

// 经纪商的权威状态：图表只负责显示它，从不自行更改。
layer.setOrders([{
  id: 'ob1',
  side: 'buy',
  type: 'limit',
  status: 'working',
  timeInForce: 'good-till-cancelled',
  quantity: 2,
  filledQuantity: 0,
  price: 96,
  revision: 1,
  permissions: { canModify: true, canCancel: true },
  label: 'BID',
}]);

layer.setPositions([{
  id: 'pos1',
  side: 'long',
  quantity: 3,
  averagePrice: 100,
  revision: 1,
  pnl: { realized: 0, unrealized: 45, currency: 'USD', markPrice: 101.5 },
  permissions: { canClose: true, canReverse: true, canProtect: true },
}]);

layer.setQuote({ time: 1704326400, bidPrice: 100.75, bidSize: 5, askPrice: 101, askSize: 4 });

// 意图交给宿主，宿主再把结果回传给该层。
layer.subscribeIntents(intent => {
  broker.send(intent).then(
    () => layer.resolveIntent({ intentId: intent.intentId, status: 'accepted' }),
    (error: Error) => layer.resolveIntent({
      intentId: intent.intentId,
      status: 'rejected',
      reason: error.message,
    }),
  );
});

// Ctrl + 左键为限价买入，Ctrl + 右键为限价卖出。
new TradingOrderPlacementAdapter(chart, layer, { quantity: 1, orderType: 'limit', modifier: 'ctrl' });
```

`setOrders`、`setPositions`、`setExecutions` 和 `setQuote` 会整体替换相应的集合。每次调用都会先规范化再与当前状态比较：如果什么都没变，就不调用订阅者，否则发布一次变更，其中包含 `added`、`updated`（`previous` / `current` 成对）、`removed` 和 `orderChanged` 字段。报价是个例外：它的变更只有 `previous` 和 `current`。当前快照由 `state()` 给出——即 `{ version, orders, positions, executions, quote }`。

规范化是严格的：价格必须落在 `tickSize` 网格上（带 `priceOrigin` 偏移），数量必须落在 `quantityStep` 上（如果设置了它）；限价单需要 `price`，止损单需要 `stopPrice`，止损限价单则两个字段都需要。不一致的数据会以异常拒绝，而不是被悄悄修正。

## 意图

该层不执行用户操作，而是把它们作为意图发布：`request*` 方法返回一个意图对象，把它放入待处理队列，并传给 `subscribeIntents` 的订阅者。宿主在经纪商处执行该意图，并通过调用 `resolveIntent({ intentId, status, reason })` 以 `accepted` 或 `rejected` 状态关闭它——结果会连同原始意图一起到达 `subscribeIntentOutcomes`。待处理的意图由 `pendingIntents()` 列出。

权限在发布之前就会检查：修改订单需要 `permissions.canModify`，撤销需要 `canCancel`，持仓操作需要 `canClose`、`canReverse` 和 `canProtect`。没有权限的订单被视为只读，调用会抛出异常。意图中会填入取自权威实体的 `expectedRevision`，以便经纪商可以拒绝那些基于过期数据构建的请求。

## 绘制与拖动

`TradingLayerPrimitive` 是图表原语，它在挂接时订阅该层，并绘制带标签的订单线、带 P&L 的持仓线、成交标记、bid/ask/last 线以及括号单之间的连线。显示什么、用什么颜色，由构造函数选项和 `applyOptions` 指定：`showOrders`、`showInactiveOrders`、`showPositions`、`showExecutions`、`showExecutionLabels`、`showQuote`、`showPnl`、`showBrackets`、`autoscale`、一组颜色（`orderBuyColor`、`orderSellColor`、`inactiveOrderColor`、`longPositionColor`、`shortPositionColor`、`executionBuyColor`、`executionSellColor`、`bidColor`、`askColor`、`lastColor`、`bracketColor`）、`lineWidth`、`fontSize`、`orderLabelSpacing`、`zOrder`，以及格式化器 `priceFormatter`、`quantityFormatter` 和 `pnlFormatter`。标识符 `id` 在创建时设置一次，之后不再改变。

当订单处于活动状态（`pending`、`working` 或 `partially-filled`）、不是市价单并且具有 `canModify` 权限时，可以用鼠标拖动它的线。拖动过程中，原语会显示对齐到网格的预览价格；松开按钮时发布 `requestModifyOrder` 意图，而预览会一直保持到宿主关闭该意图为止——被拒绝时线条回到权威价格。

光标命中的内容由 `TradingOrderHitData`、`TradingPositionHitData`、`TradingExecutionHitData` 和 `TradingQuoteHitData` 描述；`isTradingPrimitiveHitData(value)` 可以帮助在处理器中识别它们。

## 用鼠标下单

`TradingOrderPlacementAdapter` 把图表的标准下单信号与该层连接起来：它开启下单模式、监听点击并调用 `requestPlaceOrder`。它自己既不创建价格线，也不与经纪商通信。

选项：`quantity`（必填）、`orderType`——`'limit'` 或 `'stop'`（默认 `'limit'`）、`timeInForce`（默认 `'good-till-cancelled'`）、`modifier`——`'ctrl'`、`'shift'` 或 `'alt'`（默认 `'ctrl'`）、`color`、`title`、`enabled` 和 `sideResolver`。默认解析器把左键当作买入、右键当作卖出，返回 `null` 则取消下单。适配器由方法 `options()`、`applyOptions(patch)`、`setEnabled(enabled)` 和 `dispose()` 控制。

## 该层的公共方法

- `setOrders(orders)`、`setPositions(positions)`、`setExecutions(executions)`、`setQuote(quote)` — 替换权威状态；`setQuote(null)` 会移除报价。
- `state()` — 带版本号的当前状态快照。
- `normalizationOptions()` — 创建该层时使用的价格网格参数。
- `subscribeChanges(handler)`、`subscribeIntents(handler)`、`subscribeIntentOutcomes(handler)` — 订阅；每个都返回一个取消订阅的函数。
- `pendingIntents()`、`resolveIntent(resolution)` — 待处理意图队列及其关闭。
- `requestPlaceOrder(order)`、`requestModifyOrder(orderId, changes)`、`requestCancelOrder(orderId)` — 订单操作。
- `requestClosePosition(positionId, quantity)`、`requestReversePosition(positionId, quantity)` — 持仓操作；不指定数量时取整个持仓。
- `requestCreateStopLoss`、`requestEditStopLoss`、`requestRemoveStopLoss`、`requestCreateTakeProfit`、`requestEditTakeProfit`、`requestRemoveTakeProfit` — 括号单的保护性订单。
- `dispose()` — 释放资源。

## 其余导出

- 模型枚举：`TradingSide`、`ChartOrderType`、`ChartOrderStatus`、`ChartOrderTimeInForce`、`ChartPositionSide`、`ChartBracketRole`、`ChartExecutionLiquidity`、`TradingIntentKind`。
- 层与原语的枚举：`TradingLayerChangeKind`、`TradingIntentOutcomeStatus`、`TradingPrimitiveEntityKind`、`TradingQuoteKind`。
- 数据校验与规范化：`normalizeChartOrder`、`normalizeChartOrders`、`normalizeChartPosition`、`normalizeChartPositions`、`normalizeChartExecution`、`normalizeChartExecutions`、`normalizeChartQuote`、`normalizeChartOrderRequest`、`normalizeTradingIntent`、`normalizeTradingModelOptions`。
- 辅助计算：`quantizeTradingPrice` — 把任意价格对齐到网格，`chartOrderRemainingQuantity` — 订单未成交的剩余量，`chartPnlTotal` — 已实现与未实现 P&L 之和。
- 类型 `ChartOrder`、`ChartPosition`、`ChartExecution`、`ChartQuote`、`ChartOrderRequest`、`ChartOrderModification`、`TradingIntent` 以及由它们派生的意图接口。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [K线图](candlestick.md)
- [历史数据回填](backfill.md)
