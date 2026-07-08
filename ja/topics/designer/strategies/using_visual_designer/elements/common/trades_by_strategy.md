# ストラテジー別約定

![Designer The transaction strategy 00](../../../../../../images/designer_trades_strategy_00.png)

このキューブは、すべてのストラテジー約定を取得するために使用されます。

## 入力ソケット

- **Instrument** - 約定を取得する対象の銘柄。銘柄が渡されていない場合、すべてのストラテジー銘柄の約定が出力に転送されます。

## 出力ソケット

- **Trades** - 渡された銘柄から発生する約定。**Chart panel** 要素を使用したチャート表示と、**Position protection** 要素を使用したポジション保護の両方に使用できます。
