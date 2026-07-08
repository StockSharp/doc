# リスク管理

[テスト プロパティ](components/backtesting_settings.md) パネルおよび [実取引プロパティ](components/live_settings.md) パネルで、リスク管理設定を指定できます。

Risks ウィンドウでは、**Risk Rule** を選択し、**Risk Rule** の発動条件と、その **Risk Rule** の条件が発生したときに実行されるアクション (Close positions、Stop trading、Cancel orders) を設定する必要があります。

同じ種類の複数のリスク ルールを、異なるアクションで使用できます。たとえば、下のスクリーンショットでは、注文数量が 20 の場合、注文のキャンセルと取引停止のアクションが実行されます。

![Designer Risk Rule](../../../images/designer_risk_rule.png)

### リスク ルールの一覧

リスク ルールの一覧

- **P/L** - 利益/損失の大きさを監視するリスク ルール。
- **Position** - ポジションの大きさを監視するリスク ルール。
- **Position (Time)** - ポジションの存続時間を監視するリスク ルール。
- **Commission** - 手数料の大きさを監視するリスク ルール。
- **Slippage** - スリッページ量を監視するリスク ルール。
- **Order Price** - 注文価格を監視するリスク ルール。
- **Order Volume** - 注文数量を監視するリスク ルール。
- **Order (Frequency)** - 注文発注の頻度を監視するリスク ルール。
- **Error in Registration/Cancellation of Order** - 注文の登録/キャンセル時のエラー数を監視するリスク ルール。
- **Trade Price** - 約定価格を監視するリスク ルール。
- **Trade (Volume)** - 約定数量を監視するリスク ルール。
- **Trade (Frequency)** - 約定の発生頻度を監視するリスク ルール。
- **Error** - 任意のエラー数を監視するリスク ルール。

