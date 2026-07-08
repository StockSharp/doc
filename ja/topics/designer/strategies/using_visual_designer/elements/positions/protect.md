# Position Protection

![Designer Protect positions 00](../../../../../../images/designer_protect_positions_00.png)

![Designer Protect positions 01](../../../../../../images/designer_protect_positions_01.png)

このブロックは、オープン済みの取引をストップロスとテイクプロフィットで自動的に保護するために使用します。

### 入力ソケット

入力ソケット

- **Own trade** - ストップロスとテイクプロフィットで保護する必要がある約定。
- **Price** - 現在価格（ローソク足、最後のティックなどから取得できます）。銘柄の現在価格を追跡し、保護注文をアクティブ化するために必要です。

### 出力ソケット

出力ソケット

- **Take-profit** - 利益を確定するための注文。
- **Stop-loss** - 損失を限定するための注文。
- **Own transaction** - 上記のいずれかの注文によって作成された取引。

### パラメーター

Take と Stop のパラメーター

- **Value** - テイクまたはストップの値。
- **Trailing** - トレーリング保護を使用するかどうか。
- **Timeout** - 保護が市場価格で強制的にトリガーされるまでのタイムアウト値。
- **Market orders** - ポジションを迅速にクローズするために、（価格なしの）成行注文を使用します。

![Designer Protect positions 02](../../../../../../images/designer_protect_positions_02.png)

> [!WARNING]
> 入力取引は、ストラテジー全体の取引（[?????](../common/trades_by_strategy.md) ブロック）であってはなりません。これは現在ポジションの計算が不正確になるためです。保護取引もストラテジー取引になってしまいます。**Position Protection** ブロックは、[????](../orders/register.md) および [???????](modify.md) キューブの **Transaction** 出力ソケット、またはポジションを直接変更する同様のコンポーネントから取引を受け取る必要があります。
