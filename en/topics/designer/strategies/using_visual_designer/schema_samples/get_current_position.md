# Get current position

To get the volume required for flipping the current position to the opposite position, the schema used in the SMA strategy example can be used:

![Designer Determination of the volume position 00](../../../../../images/designer_determination_of_volume_position_00.png)

The **Instrument** data type is selected for the [Variable](../elements/data_sources/variable.md) cube. If the instrument is not specified, but the **Parameters** flag of the **Common** group is set, then it will be taken from the strategy, and then it will be passed to the [Position](../elements/positions/current.md).

For the [Position](../elements/positions/current.md) cube, the position property is also not specified, but the **Parameters** flag of the **Common** group is set, which means that the position will be obtained for the portfolio that is specified in the strategy settings.

After passing the instrument and changing the position, using the mathematical function with one argument (abs(pos)), the absolute value is calculated and a signal is given for the cube of variable (2). This cube contains a factor of 2, to pass the stored value through the output parameter, after which, using a mathematical formula with two arguments (abs(pos) \* 2), their product is calculated. Next, using the composite cube Conditional operator (pos \=\= 0 ? 1 : pos), the actual value of the required volume is determined, which can differ from the current position value multiplied by 2. For example, at the time of the strategy start, when no one order has been executed yet. In this case, the Conditional statement element returns a default value of 1. Because one output parameter can be connected only once with the input parameter of another element, to pass the same value between the formula and the conditional operator, an additional **Combination** cube is added.

## Recommended content

[Get order book price level](get_order_book_price_level.md)
