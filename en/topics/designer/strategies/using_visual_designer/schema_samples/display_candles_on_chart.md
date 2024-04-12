# Display candles on chart

To output candles by an instrument to a chart, the following schema can be used:

![Designer The conclusion of the candles on the chart 00](../../../../../images/designer_conclusion_of_candles_on_chart_00.png)

For the [Variable](../elements/data_sources/variable.md) cube, the **Instrument** data type is selected. If the instrument is not specified, but the **Parameters** flag of the **Common** properties group is set, it will be taken from the strategy and passed to the [Candles](../elements/data_sources/candles.md) cube. For the [Candles](../elements/data_sources/candles.md) cube, the settings for building 5\-minute candles and passing only fully formed candles are specified.

For the [Chart](../elements/common/chart.md) cube, one graphic element with a candle type was added, for which the input parameter was automatically added.

After adding the necessary graphic elements to the chart panel, the connection of the [Candles](../elements/data_sources/candles.md) and [Chart](../elements/common/chart.md), elements is added, through which the built candles will be passed for output to the chart.

## Recommended content

[Get best price for instrument](get_best_price_for_instrument.md)
