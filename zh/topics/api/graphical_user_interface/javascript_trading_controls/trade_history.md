# 成交历史

`TradeHistoryWidget` 是当前投资组合的成交记录表。最新成交位于顶部；每一行显示时间、交易品种、方向、数量、价格、成交标识符和订单标识符。

## 创建和加载

```ts
import {
  TradeHistoryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const history = TradeHistoryWidget.create(
  document.querySelector<HTMLElement>('#history')!,
  {},
  { host },
);

await history.refresh();
```

创建控件时不会自动开始加载。`refresh()` 方法通过 `host.trading.portfolioId()` 获取当前投资组合，检查加载成交历史的权限，然后调用：

```ts
const portfolioId = host.trading.portfolioId();
if (portfolioId && host.allow('load trade history'))
  host.trading.api.getExecutions(portfolioId, null, 200);
```

切换投资组合时，这种方式十分重要：每次刷新前都会重新读取标识符，而不是在创建面板时将其保存下来。

## 行为

该面板为只读。用户可以对数据行排序和进行选择、打开上下文菜单、刷新数据以及将可见列导出为 XLSX。加载错误会传给 `host.log`。

该控件的公共 API 由 `refresh(): Promise<void>` 和 `dispose(): void` 组成。

## 另请参阅

- [JavaScript 交易控件](../javascript_trading_controls.md)
- [活动订单](active_orders.md)
- [成交明细](trade_feed.md)
