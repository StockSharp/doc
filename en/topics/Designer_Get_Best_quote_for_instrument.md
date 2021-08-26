# Get best price for instrument

To register a purchase order at the current best price for the instrument, the following schema can be used:

![Designer Get the best rates for the tool 00](~/images/Designer_Get_best_quote_for_instrument_00.png)

For the [Variable](Designer_Variable.md) cube, the **Instrument** data type is selected. If the instrument is not specified, but the **Parameters** flag of the **Common** property group is set, then it will be taken from the strategy and passed to the [Order book](Designer_Depth.md) cube. The [Order book](Designer_Depth.md) cube, after receiving the current instrument from the variable, passes the order book changes by the selected instrument through the output parameter. When receiving order book changes, the [Converter](Designer_Converter.md) cube chooses the current value of the best purchase price from them.

## Recommended content

[Get current position](Designer_Determination_of_volume_position.md)
