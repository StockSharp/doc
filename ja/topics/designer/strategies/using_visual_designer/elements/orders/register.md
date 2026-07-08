# 注文登録

![Designer Position opening 00](../../../../../../images/designer_position_opening_00.png)

"Order Registration" コンポーネントは、選択した銘柄の取引注文を発注するために使用します。

## 入力ソケット

- **Instrument** - 注文対象として選択された銘柄。
- **Price** - 指値注文の価格を指定します。
- **Trigger** - 注文のアクティベーションシグナル。`False` 以外の任意の値を受け付けます。
- **Volume** - 注文する銘柄の数量。
- **Portfolio** - 注文が発注されるポートフォリオ。

## 出力ソケット

- **Order** - 発注された注文に関する情報。
- **Error** - 注文登録中のエラーに関する情報。
- **Transaction** - 注文で行われた取引に関する情報。
- **Cancellation** - 注文がキャンセルされたことを示すシグナル。
- **Executed** - 注文が完全に約定したことを示すシグナル。
- **Completed** - 注文のエラー、キャンセル、または完全約定のイベントをまとめたシグナル。

## パラメーター

- **Direction** - 注文が買いか売りかを決定します。
- **Market Order** - 注文が成行注文かどうかを示します。
- **Zero Price** - 価格がゼロに設定されている場合、注文は成行注文として登録されます。
- **Lifetime** - 指値注文が有効な期間。

## 条件付き注文の設定

**Conditional Order** - 現在の市場状況に基づいて取引システムへの発注タイミングを決定する追加条件を持つ注文。

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Connection** - 注文が発注される接続。
- **Stop Order Type** - ストップ注文のタイプ。
- **Result** - 実行されたストップ注文の結果。
- **Instrument Identifier** - 別の銘柄に関連する条件を持つストップ注文で使用する銘柄の識別子。
- **Stop Price Condition** - ストップ価格条件。"Stop price for another instrument" のような注文で使用します。
- **Stop Price** - ストップ注文をトリガーする条件を設定するストップ価格。
- **Stop-Limit Price** - Stop Price と同様ですが、"Take-profit and stop-limit" タイプの注文でのみ使用されます。
- **Stop-Limit at Market Price** - "Stop-Limit" 注文が市場価格で実行されるかどうかを示します。
- **Condition Check Interval** - 指定された期間内でのみ注文条件を確認するための時間間隔（null の場合は確認しません）。"Take-profit and stop-limit" および "Take-profit and stop-limit by order" タイプで使用されます。
- **Conditional Order Execution Identifier** - 約定に基づく条件付き注文の識別子。
- **Direction of Conditional Order by Execution** - 約定に基づく条件付き注文の方向。
- **Activation on Partial Execution** - 注文の一部約定を考慮します。"on-execution" 注文は、条件注文の一部約定時にアクティブ化されます。
- **Executed Volume** - ストップ注文を発注する数量として、注文の約定数量を使用します。"on-execution" 注文の証券数量は、条件注文の約定数量として取得されます。
- **Price of Linked Order** - 紐付けられた指値注文の価格。
- **Withdrawal on Partial Execution** - 紐付けられた指値注文の一部約定時にストップ注文を取り下げることを示します。
- **Offset from Maximum** - 直近取引の最大（最小）価格からのオフセット量。
- **Protective Spread** - 保護スプレッドのサイズ。
- **Take-Profit at Market Price** - "Take-Profit" 注文が市場価格で実行されるかどうかを示します。

## 注記

注文を扱うことは、ポジションを管理する低レベルの方法です。より高レベルの管理には、[ポジション変更](../positions/modify.md) で説明されている "Modify Position" コンポーネントを使用することを推奨します。

## 関連項目

[ポジション変更](../positions/modify.md)
