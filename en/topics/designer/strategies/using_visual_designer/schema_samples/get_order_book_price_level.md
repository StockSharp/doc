# Get order book price level

To get the required purchase string from the order book, the following schema can be used:

![Designer Event model 00](../../../../../images/designer_event_model_00.png)

The **Instrument** data type is selected for the [Variable](../elements/data_sources/variable.md) cube. If the instrument is not specified, but the **Parameters** flag of the **Common** property group is set, then it will be taken from the strategy. For the [Converter](../elements/converters/converter.md) cube, the data type and the corresponding field of the cubes collection for Bids purchase are selected. The indexer cube gets the required element from the collection of the best prices for the purchase. To get a certain price or volume value at a level, you can use the [Converter](../elements/converters/converter.md) cube.

## Recommended content

[Gallery](../../../strategy_gallery.md)
