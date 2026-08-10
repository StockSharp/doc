# 活动订单

`ActiveOrdersWidget` 显示全部订单及其当前状态。已成交、已撤销和已拒绝的记录仍会保留在表格中，因此用户可以查看本次会话内的完整变更过程。

## 创建

```ts
import {
  ActiveOrdersWidget,
  OrderStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let orders!: ActiveOrdersWidget;

orders = ActiveOrdersWidget.create(
  document.querySelector<HTMLElement>('#orders')!,
  {},
  {
    host,
    cancelOrder: id => console.log('撤销', id),
    dismissOrder: id => orders.removeOrder(id),
    editOrderField: (id, field) => {
      if (field === 'quantity' || field === 'limitPrice' || field === 'stopPrice')
        orders.startInlineEdit(id, field);
    },
    replaceOrder: (id, quantity, limitPrice, stopPrice) =>
      console.log('修改', id, quantity, limitPrice, stopPrice),
    cancelAllOrders: () => console.log('全部撤销'),
    refreshOrders: () => orders.update([]),
  },
);

orders.update([{
  id: 501,
  localId: 1,
  instrument: 'BTC@IMEX',
  side: 0,
  type: 0,
  quantity: 0.01,
  limitPrice: 68_400,
  status: OrderStates.Active,
}]);
```

`update` 会替换所有数据行。对于流式变更，请使用 `applyDelta(order)`；若要删除一行，请使用 `removeOrder(orderId)`。

## 编辑和操作

当订单处于 `Sent` 或 `Active` 状态时，始终可以双击修改数量。只有原始 `limitPrice` 大于零时才能修改限价，只有原始 `stopPrice` 大于零时才能修改止损价。确认后，控件会调用 `replaceOrder`，并同时传入完整的 `quantity`、`limitPrice`、`stopPrice` 三个值，而非只传入发生变化的字段。

对于活动订单，操作按钮会调用 `cancelOrder`。对于终止状态的记录，它会调用 `dismissOrder`，并且只从本地视图中移除该记录。拒绝原因 `rejectReason` 会显示在工具提示中。

该控件还支持撤销全部订单、刷新、排序、行选择、上下文菜单以及导出为 XLSX。

## 公共方法

- `update(orders)` — 替换所有数据行。
- `applyDelta(order)` — 添加或更新一笔订单。
- `removeOrder(orderId)` — 删除一行。
- `getOrder(orderId)` — 获取当前数据行。
- `startInlineEdit(orderId, field)` — 开始编辑 `quantity`、`limitPrice` 或 `stopPrice`。
- `dispose()` — 释放控件资源。

`OrderStates` 对象导出 `PendingRisk`、`Sent`、`Active`、`PartiallyFilled`、`Filled`、`Rejected` 和 `Cancelled` 状态。

## 另请参阅

- [JavaScript 交易控件](../javascript_trading_controls.md)
- [持仓](positions.md)
- [成交历史](trade_history.md)
