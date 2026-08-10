# 订单簿

`OrderBookWidget` 显示买卖档位、中间价、价差、累计数量和市场情绪。它支持对角和堆叠视图、买卖盘反转、5 档或 10 档深度以及 canvas 深度图。

## 创建

```ts
import {
  OrderBookWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const book = OrderBookWidget.create(
  document.querySelector<HTMLElement>('#order-book')!,
  { followsActive: true },
  {
    host,
    onPriceSelected: (price, side) =>
      console.log('预填', price, side),
    onPriceExecuted: (price, side) =>
      console.log('执行', price, side),
    maxDepth: () => 10,
    pixelRatio: () => window.devicePixelRatio || 1,
  },
);

book.setSymbol('BTC@IMEX');
```

普通单击档位会调用 `onPriceSelected`，按住 Ctrl 或 Cmd 单击则会调用 `onPriceExecuted`。数字方向 `0` 表示买入，`1` 表示卖出：单击卖价选择买入，单击买价选择卖出。控件会将交易意图传给宿主，但本身不会发送订单。

## 订单簿快照和增量

第一帧必须是完整快照：

```ts
book.applyFrame({
  symbol: 'BTC@IMEX',
  sequence: 1,
  isSnapshot: true,
  bids: [
    { price: 68_420.4, quantity: 2.5 },
    { price: 68_420.3, quantity: 4.1 },
  ],
  asks: [
    { price: 68_420.5, quantity: 1.8 },
    { price: 68_420.6, quantity: 3.2 },
  ],
});
```

之后带有 `isSnapshot: false` 的帧会作为增量应用。数量为 `0` 时删除相应档位。`sequence` 必须连续递增；如果出现缺口，控件会调用 `host.trading.marketData.resubscribe(symbol, MarketDataLevels.Full)` 以获取新快照。无效档位和买卖价格交叉的订单簿会传给 `host.log`。

## 视图和状态

```ts
book.setDepth(10);
book.setView('stacked');
book.setInvertSides(false);
book.setShowDepthChart(true);
```

`maxDepth()` 允许宿主为较小屏幕减少档位数量，`pixelRatio()` 则设置 canvas 后备缓冲区的像素密度。图表颜色由 `host.presentation.canvasPalette()` 返回。

对于跟随当前交易品种的面板，视图设置会保存在 `host.preferences` 中。固定的实例将其保存在面板状态中。创建时可以传入 `symbol`、`depth`、`view`、`invertSides`、`showDepthChart` 和 `followsActive`。

来自 `host.trading.marketData.getOrders()` 的活动订单会在对应档位旁标记出来。

## 公共方法

- `setSymbol`、`getSymbol` — 管理交易品种。
- `setDepth`、`getDepth` — 设置并返回深度。
- `setView`、`setInvertSides`、`setShowDepthChart` — 更改视图。
- `getBids`、`getAsks` — 返回当前档位。
- `isFollowsActive` — 指示面板是否跟随当前交易品种。
- `applyFrame` — 应用快照或增量。
- `dispose` — 释放资源。

## 另请参阅

- [JavaScript 交易控件](../javascript_trading_controls.md)
- [自选交易品种列表](watchlist.md)
- [订单输入](order_entry.md)
