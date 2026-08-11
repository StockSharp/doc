# 注文入力

`OrderEntryWidget` は、買いと売りの 2 面式パネルです。成行、指値、逆指値、ストップリミットの各注文に対応し、選択した注文種類に関係するフィールドだけを表示して、ホストへ渡す前に値を検証します。

## 作成と銘柄の設定

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
      console.log('送信', side, values),
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

`OrderEntryTypes` には `Market`、`Limit`、`Stop`、`StopLimit` が含まれ、`OrderEntrySides` には `Buy` と `Sell` が含まれます。

## 検証と送信

コントロールは、正の値、ロットサイズ、呼値、最小数量、最大数量を検証します。任意の利食いと損切りは、`toggleTpSl` メソッドで有効にします。

`submit(side)` は最初に `validate(side)` を呼び出します。データが正しい場合、`submitOrder` ハンドラーは売買区分と次のオブジェクトを受け取ります。

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

コントロール自体は、ポートフォリオの選択、接続の確認、サーバーへの注文送信を行いません。これらの処理はホストのハンドラーが担当します。

## 価格、数量、状態

- `setBbo(bid, ask)` は最良買気配と最良売気配を更新します。
- `applyBbo(side)` は、即時約定のために選択した列へ BBO の反対側の価格を設定します。
- `setAvailable(side, value)` は、表示する利用可能残高を設定します。
- `setMaxQuantity(side, value)` は、割合ボタンで数量を計算するときの最大値を設定します。
- `applyPercent(side, pct)` は、`setMaxQuantity` で設定した最大数量に対して、指定された割合を適用します。
- `preselect(side)` は、「クリックで取引」操作後の売買区分を選択します。
- `setEnabled(false)` は、入力済みの値を破棄せずに送信を禁止します。
- `getValues(side)` と `validate(side)` を使用すると、外部からフォームを確認できます。

静的メソッド `toApiType` は、`Limit` を `0`、`Market` を `1`、`Stop` と `StopLimit` を `2` に変換します。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [板情報](order_book.md)
- [ポジション](positions.md)
