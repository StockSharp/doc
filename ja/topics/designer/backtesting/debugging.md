# デバッグ

ストラテジーのテスト過程では、特定のキューブの入力にどのデータが入るのか、またはその出力に何が渡されるのかを確認する必要がしばしばあります。そのために、[Designer](../../designer.md) には **Debugger** が用意されています。

![Designer Debug 00](../../../images/designer_debug_00.png)

**Emulation** Ribbon の **Debugger** グループには、次のボタンがあります。

- ![Designer Debug 01](../../../images/designer_debug_01.png)**Add breakpoint** – 選択した要素にブレークポイントを追加します。ブレークポイントが追加された要素は赤い枠で強調表示されます。
- ![Designer Debug 02](../../../images/designer_debug_02.png)**Delete breakpoint** – ブレークポイントを削除します。
- ![Designer Debug 03](../../../images/designer_debug_03.png)**Next element** – ブレークポイントがトリガーされたとき、スキームの次の要素へ移動します。
- **Step to out** – ブレークポイントがトリガーされたとき、現在の要素の出力へ移動します。要素出力で渡された値を確認するために使用します。
- ![Designer Debug 04](../../../images/designer_debug_04.png)**Step in** – ブレークポイントがトリガーされたとき、複合要素の内部へ移動します。複合要素のスキームを自動的に開き、データが最初に送信される要素で停止します。
- ![Designer Debug 05](../../../images/designer_debug_05.png)**Step out** – ブレークポイントがトリガーされ、複合要素内にいる場合、開いている複合要素が使用されている 1 つ上のレベルへ戻ります。
- ![Designer Debug 06](../../../images/designer_debug_06.png)**Continue** – 次のブレークポイントがトリガーされるまで続行します。

## 推奨コンテンツ

[ブレークポイント](debugging/break_points.md)
