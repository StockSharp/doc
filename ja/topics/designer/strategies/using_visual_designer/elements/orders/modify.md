# 注文変更

![Designer Moving applications 00](../../../../../../images/designer_moving_applications_00.png)

このブロックは、銘柄の注文を変更するために使用します。

### 入力ソケット

入力ソケット

- **Trigger** - 注文を移動するタイミングを判定するシグナル。
- **Order** - 変更される注文。
- **Price** - 新しい価格の数値。
- **Volume** - 新しい数量の数値。

### 出力ソケット

出力ソケット

- **Order** - 変更された注文。この注文は、**Transactions by Order** 要素を使用してその取引を取得したり、**Chart Panel** ブロックを使用してチャートに表示したりするために使用できます。
- **Error** - 注文移動時のエラー。
- **Trade** - 発注された注文の約定。

パラメーター

- **Zero Price** - ゼロ価格は成行注文を登録します。

## 関連項目

[注文キャンセル](cancel.md)
