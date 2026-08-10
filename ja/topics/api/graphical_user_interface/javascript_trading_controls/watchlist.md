# ウォッチリスト

`WatchlistWidget` は、銘柄とリアルタイム気配値を表示します。検索、お気に入り、カテゴリー、アクティブな銘柄の選択に対応し、実際に表示されているシンボルだけを購読します。

## 作成

```ts
import {
  WatchlistWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let watchlist!: WatchlistWidget;

watchlist = WatchlistWidget.create(
  document.querySelector<HTMLElement>('#watchlist')!,
  {},
  {
    host,
    onSelect: symbol => {
      watchlist.setCurrentSymbol(symbol);
      console.log('選択', symbol);
    },
  },
);
```

作成時には `init()` が自動的に開始され、`host.trading.api.searchInstruments('')` を通じて一覧を読み込みます。各レコードでは `symbol`、`name`、`exchange`、`category` の各フィールドが使用されます。

価格ストリームは外部から渡します。

```ts
watchlist.onPriceUpdate('BTC@IMEX', 68_420.5);
```

このメソッドは価格と騰落率の対象セルだけを更新し、変化のアニメーションを維持します。`setCurrentSymbol(symbol)` は現在の銘柄を強調表示します。

## 検索、カテゴリー、購読

検索ではシンボル、名称、取引所が照合されます。すべての銘柄、お気に入り、検出されたカテゴリーごとにタブが作成されます。お気に入りのシンボルは `host.preferences` に保存されます。

コントロールは、表示中の先頭 30 銘柄を `MarketDataLevels.Quotes` レベルで購読します。画面に描画される行は最大 300 行ですが、フィルタリングとエクスポートは検索結果全体を対象にします。`host.isPrimary === true` のインスタンスだけが、表示中の気配値を `host.ticker` を通じて公開します。

騰落率は、UTC での当日に最初に受信した価格を基準に計算されます。この基準価格はキャッシュデータであるため、ユーザー設定ではなく `host.cache` に保存されます。

## 公開メソッド

- `init()` — 銘柄を読み込んで購読を準備します。`create` から自動的に呼び出されます。
- `setCurrentSymbol(symbol)` — アクティブな銘柄を設定します。
- `onPriceUpdate(symbol, price)` — 新しい価格を適用します。
- `dispose()` — 購読を解除し、リソースを解放します。

## 関連項目

- [JavaScript トレーディングコントロール](../javascript_trading_controls.md)
- [板情報](order_book.md)
- [注文入力](order_entry.md)
