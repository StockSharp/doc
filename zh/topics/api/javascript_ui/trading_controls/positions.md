# 持仓

`PositionsWidget` 显示未平仓持仓，并在顶部固定一行现金余额。默认情况下，持仓按字母顺序排列，而余额不参与排序、选择和导出。

## 创建和更新

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let positions!: PositionsWidget;

positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('平仓', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('反向开仓', portfolioId, instrumentId, symbol),
    refreshPositions: () => positions.update([]),
  },
);

positions.update([{
  portfolioId: 10,
  instrumentId: 42,
  instrument: 'BTC@IMEX',
  quantity: 0.25,
  avgPrice: 68_000,
  currentPrice: 68_420,
  unrealizedPnl: 105,
  realizedPnl: 20,
}]);

positions.updateBalance({
  available: 48_251,
  locked: 1_749,
  total: 50_000,
});
```

`update` 会替换持仓列表。`applyDelta` 根据投资组合和交易品种的组合更新一条持仓；数量为零的行会被删除。`updateBalance(null)` 会移除固定的余额行。

## 数据和操作

每一行显示数量、平均价和当前价，以及由 `realizedPnl` 与 `unrealizedPnl` 相加得到的总盈亏。其颜色类由 `host.presentation.pnlClass` 返回。

行中的按钮会调用宿主传入的 `closePosition` 和 `reversePosition`。控件本身不会创建或发送交易订单。

## 公共方法

- `update(positions)` — 替换全部持仓。
- `updateBalance(balance)` — 设置或移除现金余额。
- `applyDelta(position)` — 应用一条持仓的流式变更。
- `dispose()` — 释放资源。

面板还支持刷新、排序、上下文菜单以及将持仓导出为 XLSX。

## 另请参阅

- [JavaScript 交易控件](../trading_controls.md)
- [活动订单](active_orders.md)
- [订单输入](order_entry.md)
