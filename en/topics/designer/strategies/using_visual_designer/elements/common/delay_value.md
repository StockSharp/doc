# Value Delay

![Designer Delay 00](../../../../../../images/designer_delay_00.png)

This component is used to delay the transmission of a value for a specified number of iterations.

## Input Sockets

- **Trigger** – A signal (any value except `False`) that initializes the internal counter to start the delay countdown.
- **Input** - Any incoming value (except for [unfinished candles](../data_sources/candles.md) or [non-final indicator values](indicator.md)) that decreases the internal counter. When the counter reaches zero, it is deactivated, and the outgoing socket is activated. If the counter was not activated by the **Trigger**, incoming values are ignored.

## Output Sockets

- **Signal** – Outputs a signal when the counter reaches zero, indicating the end of the delay.

## Parameters

- **Duration** - Specifies the delay duration in iterations.

![Designer Delay 01](../../../../../../images/designer_delay_01.png)

## See Also

- [Comparison](comparison.md)