# 有効注文

`ActiveOrdersWidget` は、すべての注文と現在の状態を表示します。約定済み、取消済み、拒否された行もテーブルに残るため、ユーザーはセッション中の変更履歴全体を確認できます。

## 作成

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
    cancelOrder: id => console.log('取消', id),
    dismissOrder: id => orders.removeOrder(id),
    editOrderField: (id, field) => {
      if (field === 'quantity' || field === 'limitPrice' || field === 'stopPrice')
        orders.startInlineEdit(id, field);
    },
    replaceOrder: (id, quantity, limitPrice, stopPrice) =>
      console.log('変更', id, quantity, limitPrice, stopPrice),
    cancelAllOrders: () => console.log('全注文を取消'),
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

`update` は行セット全体を置き換えます。ストリーミング更新には `applyDelta(order)`、1 行の削除には `removeOrder(orderId)` を使用します。

## 編集と操作

注文が `Sent` または `Active` の状態にある間は、数量をいつでもダブルクリックで変更できます。指値を編集できるのは既存の `limitPrice` が 0 より大きい場合だけで、逆指値を編集できるのは既存の `stopPrice` が 0 より大きい場合だけです。確定すると、コントロールは `replaceOrder` を呼び出し、変更したフィールドだけでなく、`quantity`、`limitPrice`、`stopPrice` の 3 つすべてを渡します。

有効注文では、操作ボタンが `cancelOrder` を呼び出します。終端状態の行では `dismissOrder` を呼び出し、ローカル表示からだけレコードを削除します。拒否理由 `rejectReason` はツールチップに表示されます。

コントロールには、全注文の取消、更新、並べ替え、行選択、コンテキストメニュー、XLSX へのエクスポートも用意されています。

## 公開メソッド

- `update(orders)` — すべての行を置き換えます。
- `applyDelta(order)` — 1 件の注文を追加または更新します。
- `removeOrder(orderId)` — 行を削除します。
- `getOrder(orderId)` — 現在の行を取得します。
- `startInlineEdit(orderId, field)` — `quantity`、`limitPrice`、`stopPrice` の編集を開始します。
- `dispose()` — コントロールのリソースを解放します。

`OrderStates` オブジェクトは、`PendingRisk`、`Sent`、`Active`、`PartiallyFilled`、`Filled`、`Rejected`、`Cancelled` の各状態をエクスポートします。

## 関連項目

- [JavaScript トレーディングコントロール](../javascript_trading_controls.md)
- [ポジション](positions.md)
- [約定履歴](trade_history.md)
