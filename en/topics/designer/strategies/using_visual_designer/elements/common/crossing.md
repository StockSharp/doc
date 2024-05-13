# Crossing

![Designer Crossing 00](../../../../../../images/designer_crossing_00.png)

This element is used for tracking the position of two values relative to each other. For example, to determine the moment of crossing between two lines.

The comparison is made regarding the values on two sockets **Up** and **Down**.

## Incoming Sockets

- **Up** – values that allow comparison (for example, a numerical value, indicator value, etc.).
- **Down** – values that allow comparison (for example, a numerical value, indicator value, etc.).

## Outgoing Sockets

- **Flag** – true if **Up** is greater than **Down**, otherwise false.

![Designer Crossing 01](../../../../../../images/designer_crossing_01.png)

An example of using the Crossing block to track the crossings of two [SMA indicators](../../../../../api/indicators/list_of_indicators/sma.md). Two Crossing blocks are used, and each outputs true separately depending on when the long SMA is greater than the short one and when it is less.

## See Also

[Value Delay](delay_value.md)