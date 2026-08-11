# 約定履歴

`TradeHistoryWidget` は、現在のポートフォリオの約定を表示するテーブルです。新しい約定が上に並び、各行には時刻、銘柄、売買区分、数量、価格、約定識別子、注文識別子が表示されます。

## 作成と読み込み

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

コントロールを作成しても、読み込みは自動的に開始されません。`refresh()` メソッドは `host.trading.portfolioId()` から現在のポートフォリオを取得し、ホストが約定履歴の読み込みを許可した場合、次を呼び出します。

```ts
const portfolioId = host.trading.portfolioId();
if (portfolioId && host.allow('load trade history'))
  host.trading.api.getExecutions(portfolioId, null, 200);
```

この方式はポートフォリオを切り替える場合に重要です。識別子はパネルの作成時に保存されるのではなく、更新のたびに直前で読み取られます。

## 動作

このパネルは読み取り専用です。ユーザーは行の並べ替えと選択、コンテキストメニューの表示、データの更新、表示列の XLSX エクスポートを実行できます。読み込みエラーは `host.log` へ渡されます。

コントロールの公開 API は、`refresh(): Promise<void>` と `dispose(): void` です。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [有効注文](active_orders.md)
- [歩み値](trade_feed.md)
