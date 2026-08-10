# Active Orders

`ActiveOrdersWidget` shows the complete list of orders and their current states. Filled, cancelled, and rejected rows remain in the table, so the user can see the entire sequence of changes within the session.

## Creating the control

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
    cancelOrder: id => console.log('cancel', id),
    dismissOrder: id => orders.removeOrder(id),
    editOrderField: (id, field) => {
      if (field === 'quantity' || field === 'limitPrice' || field === 'stopPrice')
        orders.startInlineEdit(id, field);
    },
    replaceOrder: (id, quantity, limitPrice, stopPrice) =>
      console.log('replace', id, quantity, limitPrice, stopPrice),
    cancelAllOrders: () => console.log('cancel all'),
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

`update` replaces the full row set. For streaming changes, use `applyDelta(order)`; to remove one row, use `removeOrder(orderId)`.

## Editing and actions

While the order is in the `Sent` or `Active` state, its quantity can always be changed by double-clicking. The limit price is editable only when the existing `limitPrice` is greater than zero, and the stop price only when the existing `stopPrice` is greater than zero. After confirmation, the control invokes `replaceOrder` and passes the complete `quantity`, `limitPrice`, and `stopPrice` triplet rather than only the changed field.

For an active order, the action button invokes `cancelOrder`. For a terminal row, it invokes `dismissOrder` and removes the record only from the local view. The `rejectReason` is shown in a tooltip.

The control also provides cancel all, refresh, sorting, row selection, a context menu, and XLSX export.

## Public methods

- `update(orders)` — replace all rows.
- `applyDelta(order)` — add or update one order.
- `removeOrder(orderId)` — remove a row.
- `getOrder(orderId)` — retrieve the current row.
- `startInlineEdit(orderId, field)` — start editing `quantity`, `limitPrice`, or `stopPrice`.
- `dispose()` — release the control's resources.

The `OrderStates` object exports the `PendingRisk`, `Sent`, `Active`, `PartiallyFilled`, `Filled`, `Rejected`, and `Cancelled` states.

## See also

- [JavaScript Trading Controls](../javascript_trading_controls.md)
- [Positions](positions.md)
- [Trade history](trade_history.md)
