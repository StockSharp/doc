# ポジション

`PositionsWidget` は、未決済ポジションと、上部に固定された現金残高の行を表示します。ポジションは既定で銘柄名順に並び、残高は並べ替え、選択、エクスポートの対象になりません。

## 作成と更新

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
      console.log('決済', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('ドテン', portfolioId, instrumentId, symbol),
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

`update` はポジション一覧を置き換えます。`applyDelta` はポートフォリオと銘柄の組み合わせで 1 件のポジションを更新し、数量がゼロの行を削除します。`updateBalance(null)` は固定残高を削除します。

## データと操作

行には数量、平均価格、現在価格に加えて、`realizedPnl` と `unrealizedPnl` の合計として計算された PnL が 1 つ表示されます。その色のクラスは `host.presentation.pnlClass` が返します。

行のボタンは、ホストから渡された `closePosition` と `reversePosition` を呼び出します。コントロール自体は取引注文を作成したり送信したりしません。

## 公開メソッド

- `update(positions)` — すべてのポジションを置き換えます。
- `updateBalance(balance)` — 現金残高を設定または削除します。
- `applyDelta(position)` — 1 件のポジションにストリーミング更新を適用します。
- `dispose()` — リソースを解放します。

パネルは、更新、並べ替え、コンテキストメニュー、ポジションの XLSX エクスポートにも対応します。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [有効注文](active_orders.md)
- [注文入力](order_entry.md)
