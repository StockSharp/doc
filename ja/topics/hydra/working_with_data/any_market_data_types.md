# 任意のマーケットデータ型

[Hydra](../../hydra.md) では、さまざまな種類のマーケットデータを取得するために、代替のデータ型を使用できます。

これは、ソースが必要なマーケットデータのダウンロードに対応していない場合に必要です。たとえば、**板情報** を構築するために、複数の種類のマーケットデータを同時に使用できます。

重要\! **板情報** は、これらのデータ型に最良気配価格が含まれている場合に限り、**注文ログ** または **Level 1** から構築できます。

**Level 1** の値は、リアルタイムのマーケットデータを提供する任意のソースからダウンロードできることに注意してください。**Level 1** は、**板情報** から [変換](../tasks/converter.md) して受信することもできます。

構築するには、次の手順を実行します。

1. マーケットデータを取得する期間と銘柄を選択します。![hydra LEVEL 1 build depth data](../../../images/hydra_level1_build_depth_data.png)
2. **構築元** フィールドを選択し、必要なデータ型を選択します。![hydra type build data](../../../images/hydra_type_build_data.png)

   重要\! ローソク足を構築するためのソースとして **板情報、注文ログ、Level 1** を選択すると、追加パラメーターの選択項目が表示されます。![hydra ext proper build data](../../../images/hydra_ext_proper_build_data.png)
3. パラメーターを設定した後、![hydra candles](../../../images/hydra_candles.png) ボタンをクリックします。![hydra LEVEL 1 build depth data result](../../../images/hydra_level1_build_depth_data_result.png)

**Candles** を構築する場合は、より小さい時間枠のローソク足から、より大きい時間枠のローソク足を構築するオプションも利用できます。 

たとえば、時間枠が 1 分のローソク足がある場合、**構築元** 行で適切な種類を選択することで、それらから時間枠が 5 分のローソク足を構築できます。

**[動画チュートリアル](../videos/building_order_books.md)を見る**
