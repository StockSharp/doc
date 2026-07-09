# ローソク足の生成

[Hydra](../../hydra.md) では、ダウンロード済みの約定に基づいてさまざまな種類のローソク足を生成でき、その後 [Excel](https://en.wikipedia.org/wiki/Excel)、XML、SQL、BIN、JSON、または TXT 形式へエクスポートできます。

これにより、生成されたデータを任意のテクニカル分析プログラム (WealthLab、AmiBroker など) で使用できます。

## ローソク足生成プロセス

1. **全般** タブで **Candles** ボタンをクリックすると、次のウィンドウが開きます。

   ![hydra candles main](../../../images/hydra_candles_main.png)

2. 開いたウィンドウで、ローソク足生成パラメーターを設定する必要があります。

   - ドロップダウンリストから目的のローソク足タイプを選択します (すべての [標準ローソク足タイプ](../../api/candles.md) がサポートされています)
   - 選択したローソク足タイプに必要なパラメーターを指定します。
     - [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) の場合 - **Timeframe** を選択します
     - [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) の場合 - **Volume** を指定します
     - [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) の場合 - **Number of ticks** を指定します
     - [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) の場合 - **Range** を指定します
     - [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) の場合 - **Block size** を指定します
     - [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) の場合 - **P&F Parameters** を指定します
   - ローソク足を生成する銘柄を選択します
   - 時間範囲を指定します (必要な場合)
   - ![hydra find](../../../images/hydra_find.png) ボタンをクリックして生成を開始します

### 時間枠ローソク足生成の例

AAPL@NASDAQ 銘柄の 5 分足を生成するには、次の手順を実行します。

1. ローソク足タイプ [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) を選択します
2. **Timeframe** = 5 min に設定します
3. AAPL@NASDAQ 銘柄を選択します
4. 検索ボタンをクリックします

データ生成後、結果が表示されます。

![hydra candles tf](../../../images/hydra_candles_tf.png)

### ボリュームローソク足生成の例

ボリュームローソク足を生成するには、次の手順を実行します。

1. ローソク足タイプ [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) を選択します
2. ボリュームを指定します (例: 100)
3. 銘柄を選択します
4. **Build from** フィールドで **Ticks** を選択します
5. 検索ボタンをクリックします

生成結果:

![hydra candles volume](../../../images/hydra_candles_volume.png)

## ローソク足構築用のデータソース

マーケットデータをソースから直接取得できなかった場合は、[**Build from**](any_market_data_types.md) フィールドで構築元となるデータの種類を選択して、ローソク足を生成できます。

- **Ticks** - ティックデータからローソク足を構築します
- **Order Books** - 板情報データからローソク足を構築します
- **Level1** - Level1 データからローソク足を構築します
- **Smaller Timeframe** - より小さい時間枠のローソク足から、より大きい時間枠のローソク足を構築します

### さまざまな構築オプションの例:

- ティックから 10 分足を構築:

  ![hydra candles tf 10](../../../images/hydra_candles_tf_10.png)

- 5 分足から 30 分足を構築:

  ![hydra candles tf 01](../../../images/hydra_candles_tf_01.png)

> [!TIP]
> **Build from** フィールドで **don't build** を選択すると、データソースを通じて直接ダウンロードされた既製のローソク足のみが検索されます。

## 生成されたローソク足の可視化

生成されたローソク足をグラフィカルに表示するには、次の手順を実行します。

1. ![hydra candles](../../../images/hydra_candles.png) ボタンをクリックします
2. 構築されたローソク足のチャートが開きます。

   ![hydra candles tf chart](../../../images/hydra_candles_tf_chart.png)

   ![hydra candles volume chart](../../../images/hydra_candles_volume_chart.png)

## チャートへのインジケーターの追加

テクニカルインジケーターをローソク足チャートに追加できます。

1. チャートパネルを右クリックしてコンテキストメニューを開きます
2. **Indicator** 項目と、リストから目的のインジケーターを選択します
3. インジケーターを別のパネルに表示するには、次の手順を実行します。
   - ![hydra add](../../../images/hydra_add.png) ボタンを使用して新しいパネルを追加します
   - コンテキストメニューから目的のインジケーターを選択します

インジケーターを追加したチャートの例:

![hydra candles ind chart](../../../images/hydra_candles_ind_chart.png)

## データのエクスポート

取得したローソク足の値は、他のプログラムで使用するために [さまざまな形式へエクスポート](export_data.md) できます。

**さまざまな種類のローソク足構築については、[動画チュートリアル](../videos/building_candles.md)も参照してください**
