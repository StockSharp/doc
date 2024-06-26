# Генерация свечек

[Hydra](../../hydra.md) позволяет на основе скачанных сделок формировать свечи, которые в последствии можно экспортировать в форматы [Excel](https://ru.wikipedia.org/wiki/Excel), xml, sql, bin, Json или txt (cм. ниже).

Это позволяет в последствии использовать данные в любых программах теханализа (WealthLab, AmiBroker и т.д.).

## Генерация свечей

1. На вкладке **Общее** нажать кнопку **Свечи**, откроется следующее окно:![hydra candles main](../../../images/hydra_candles_main.png)
2. Далее необходимо: 
   - Выбрать из выпадающего списка интересующий тип свечи (поддерживаются все [стандартные свечи](../../api/candles.md))
   - Выбрать нужный **Тайм Фрейм**.

     Для примера нами выбраны свечи с Тайс Фреймом 5 мин.
   - Выбрать инструмент (в нашем случае это SBER@TQBR) и нажать на кнопку ![hydra find](../../../images/hydra_find.png)

   После этого произойдет генерация свечей на основе найденных данных. Например, для свечей типа [TimeFrameCandle](xref:StockSharp.Algo.Candles.TimeFrameCandle) будет сформировано следующее:![hydra candles tf](../../../images/hydra_candles_tf.png)

   Если есть потребность построить свечи другого типа, например [VolumeCandle](xref:StockSharp.Algo.Candles.VolumeCandle) нужно сделать следующее: 
   - Выбрать тип свечи. ![hydra candles volume 100](../../../images/hydra_candles_volume_100.png)
   - Выбрать период объем.
   - Установить инструмент.
   - Выбрать в **Построить из**, например, **Тики**. Нажать на кнопку ![hydra find](../../../images/hydra_find.png)![hydra candles volume](../../../images/hydra_candles_volume.png)

   Если маркет\-данные не удалось получить из источника, можно сгенерировать свечи выбрав в поле ["Построить из"](any_market_data_types.md) тип данных из которых они будут построены.

   Например:
   - Свечи с Тайм Фреймом 10 мин из Тиков. ![hydra candles tf 10](../../../images/hydra_candles_tf_10.png)
   - Можно построить свечи с большим Тайм Фреймом из свечей с меньшим, выбрав в **Построить из** построить **Меньший Тайм Фрейм**. Например из свечей с Тайм Фреймом 5 минут свечи с Тайм Фреймом 30 мин.![hydra candles tf 01](../../../images/hydra_candles_tf_01.png)

   > [!TIP]
   > Если в **Построить из:** выбрать **не строить**, то будет произведен поиск готовых свечей, которые были скачаны через источник.
3. Если необходимо графически посмотреть сгенерированные свечи, то нужно нажать на кнопку ![hydra candles](../../../images/hydra_candles.png) и после этого откроется график:![hydra candles tf chart](../../../images/hydra_candles_tf_chart.png)![hydra candles volume chart](../../../images/hydra_candles_volume_chart.png)
4. К графику можно добавить индикаторы. Для этого нужно открыть контекстное меню, щелкнув правой кнопкой по панели графика, и выбрать пункт **Индикатор**. Кроме того, индикатор можно вывести на отдельной панели. Для этого необходимо добавить новую панель при помощи кнопки ![hydra add](../../../images/hydra_add.png) и также выбрать необходимый индикатор из контекстного меню.![hydra candles ind chart](../../../images/hydra_candles_ind_chart.png)
5. Полученные значения можно [экспортировать в нужный формат](export_data.md).

**Смотреть [видеоинструкцию](../videos/building_candles.md)**
