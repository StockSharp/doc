# Custom candles

The user can select a **Custom type** of candles and independently select which candles will be built, while the candles will be built "on the fly", that is, immediately.

![hydra type candle 00 00](../images/hydra_type_candle_00_00.png)

Let's consider an example of such a building. **Bitmex** exchange does not provide a possibility to receive candles with a Time Frame of 10 minutes.

![hydra type candle 00 01](../images/hydra_type_candle_00_01.png)

The sequence of obtaining such candles:

1. Select **Custom** candles
2. In the settings, we specify **TF** candles and a period of 10 minutes
3. In the source, we specify from what the candles will be built \- **Order Log** ![hydra type candle 00 02](../images/hydra_type_candle_00_02.png)
4. We set the period. As you can see, next to the candle name appeared the **Generated** indication..![hydra type candle 00 03](../images/hydra_type_candle_00_03.png)
5. Click on start and the data starts downloading.![hydra type candle 00 04](../images/hydra_type_candle_00_04.png)
6. Let's go to the candles section and [see the downloaded data](HydraViewingMarketData.md).![hydra type candle 00 06](../images/hydra_type_candle_00_06.png)

As you can see, the data has been successfully received.

Consider an example when we need to get a [RangeCandle](xref:StockSharp.Algo.Candles.RangeCandle):

1. Select **Custom** candles.
2. In the settings, specify the Range candles and the volume 10.
3. 3.In the source, we specify from what the candles will be built \- **Ticks**.![hydra type candle 00 07](../images/hydra_type_candle_00_07.png)
4. We set the period.
5. Click on start and the data starts downloading.![hydra type candle 00 08](../images/hydra_type_candle_00_08.png)
6. 6.Let's go to the candles section and [see the downloaded data](HydraViewingMarketData.md).![hydra type candle 00 09](../images/hydra_type_candle_00_09.png)
