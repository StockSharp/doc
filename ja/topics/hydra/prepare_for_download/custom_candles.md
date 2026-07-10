# カスタムローソク足

ユーザーはローソク足の **カスタムタイプ** を選択し、どのローソク足を構築するかを個別に選択できます。ローソク足は即座に構築されます。

![hydra ローソク足タイプ 00 00](../../../images/hydra_type_candle_00_00.png)

このような構築の例を見てみましょう。**Bitmex** 取引所は、時間枠が10分のローソク足を受信する機能を提供していません。

![hydra ローソク足タイプ 00 01](../../../images/hydra_type_candle_00_01.png)

このようなローソク足を取得する手順:

1. **Custom** ローソク足を選択します
2. 設定で、**TF** ローソク足と10分の期間を指定します
3. ソースで、ローソク足を何から構築するかを指定します - **注文ログ** ![hydra ローソク足タイプ 00 02](../../../images/hydra_type_candle_00_02.png)
4. 期間を設定します。見てわかるように、ローソク足名の横に **Generated** 表示が現れました。![hydra ローソク足タイプ 00 03](../../../images/hydra_type_candle_00_03.png)
5. start をクリックすると、データのダウンロードが開始されます。![hydra ローソク足タイプ 00 04](../../../images/hydra_type_candle_00_04.png)
6. ローソク足セクションに移動して、[ダウンロードされたデータを確認](../working_with_data/view_and_export.md)します。![hydra ローソク足タイプ 00 06](../../../images/hydra_type_candle_00_06.png)

見てわかるように、データは正常に受信されました。

[RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) を取得する必要がある場合の例を考えてみましょう:

1. **Custom** ローソク足を選択します。
2. 設定で、Range ローソク足とボリューム 10 を指定します。
3. ソースで、ローソク足を何から構築するかを指定します - **Ticks**。![hydra ローソク足タイプ 00 07](../../../images/hydra_type_candle_00_07.png)
4. 期間を設定します。
5. start をクリックすると、データのダウンロードが開始されます。![hydra ローソク足タイプ 00 08](../../../images/hydra_type_candle_00_08.png)
6. ローソク足セクションに移動して、[ダウンロードされたデータを確認](../working_with_data/view_and_export.md)します。![hydra ローソク足タイプ 00 09](../../../images/hydra_type_candle_00_09.png)
