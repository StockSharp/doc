# 订单输入

`OrderEntryWidget` 是一个双向买卖面板。它支持市价单、限价单、止损单和止损限价单，只显示与所选类型相关的字段，并在将值传给宿主之前进行校验。

## 创建并设置交易品种

```ts
import {
  OrderEntrySides,
  OrderEntryTypes,
  OrderEntryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const pad = OrderEntryWidget.create(
  document.querySelector<HTMLElement>('#order-entry')!,
  {},
  {
    host,
    submitOrder: (side, values) =>
      console.log('提交', side, values),
  },
);

pad.setInstrument({
  symbol: 'BTC@IMEX',
  lotSize: 0.001,
  tickSize: 0.1,
  minVolume: 0.001,
  maxVolume: 5,
});

pad.setOrderType(OrderEntryTypes.Limit);
pad.setBbo(68_420.4, 68_420.5);
pad.setAvailable(OrderEntrySides.Buy, 48_251);
pad.setMaxQuantity(OrderEntrySides.Buy, 0.7);
pad.setQuantity(0.01);
```

`OrderEntryTypes` 包含 `Market`、`Limit`、`Stop` 和 `StopLimit`，`OrderEntrySides` 则包含 `Buy` 和 `Sell`。

## 校验和提交

控件会检查正值、每手数量、价格步长以及最小和最大数量。可选的止盈和止损通过 `toggleTpSl` 方法启用。

`submit(side)` 首先调用 `validate(side)`。数据正确时，`submitOrder` 处理程序会收到方向和以下对象：

```ts
interface OrderEntryValues {
  type: 'market' | 'limit' | 'stop' | 'stoplimit';
  quantity: number;
  limitPrice: number | null;
  stopPrice: number | null;
  takeProfit: number | null;
  stopLoss: number | null;
}
```

控件本身不会选择投资组合、检查连接或向服务器发送订单。这些操作由宿主的处理程序负责。

## 价格、数量和状态

- `setBbo(bid, ask)` 更新最佳买价和卖价。
- `applyBbo(side)` 将 BBO 的另一侧价格填入所选列，以便立即成交。
- `setAvailable(side, value)` 设置显示的可用余额。
- `setMaxQuantity(side, value)` 设置百分比按钮用于计算数量的最大值。
- `applyPercent(side, pct)` 应用通过 `setMaxQuantity(side, value)` 设置的最大数量的 25%、50%、75% 或 100%。
- `preselect(side)` 在“单击交易”手势后预选方向。
- `setEnabled(false)` 禁止提交，但保留已经输入的值。
- `getValues(side)` 和 `validate(side)` 允许从外部检查表单。

静态方法 `toApiType` 将 `Limit` 转换为 `0`、`Market` 转换为 `1`，并将 `Stop` 和 `StopLimit` 转换为 `2`。

## 另请参阅

- [JavaScript 交易控件](../javascript_trading_controls.md)
- [订单簿](order_book.md)
- [持仓](positions.md)
