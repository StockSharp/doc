# Order Registration

![Designer Position opening 00](../../../../../../images/designer_position_opening_00.png)

The "Order Registration" component is used for placing trading orders for a selected instrument.

## Input Sockets

- **Instrument** – The selected instrument for the order.
- **Price** – Specifies the price for a limit order.
- **Trigger** – Activation signal for the order, accepts any value except `False`.
- **Volume** – The quantity of instruments for the order.
- **Portfolio** – The portfolio within which the order will be placed.

## Output Sockets

- **Order** – Information about the placed order.
- **Error** – Information about any error during order registration.
- **Transaction** – Information about the transaction made on the order.
- **Cancellation** – Signal that the order has been cancelled.
- **Executed** – Signal that the order has been fully executed.
- **Completed** – Signal that combines events of error, cancellation, or full execution of the order.

## Parameters

- **Direction** – Determines whether the order is for buying or selling.
- **Market Order** – Indicates whether the order is a market order.
- **Zero Price** – If the price is set to zero, the order is registered as a market order.
- **Lifetime** – The duration for which a limit order remains active.

## Conditional Order Setup

**Conditional Order** – An order with additional conditions determining the timing of placement in the trading system based on the current market situation.

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Connection** – The connection where the order will be placed.
- **Stop Order Type** – The type of stop order.
- **Result** – The outcome of the executed stop order.
- **Instrument Identifier** – The identifier for the instrument for stop orders with conditions related to another instrument.
- **Stop Price Condition** – The stop price condition. Used for orders like "Stop price for another instrument."
- **Stop Price** – The stop price that sets the condition for triggering the stop order.
- **Stop-Limit Price** – Similar to the Stop Price but used only for "Take-profit and stop-limit" type orders.
- **Stop-Limit at Market Price** – Indicates if the "Stop-Limit" order executes at the market price.
- **Condition Check Interval** – The time interval for checking the conditions of the order only within the specified period (if null, then no checks). Used for "Take-profit and stop-limit" and "Take-profit and stop-limit by order" types.
- **Conditional Order Execution Identifier** – The identifier for the conditional order based on execution.
- **Direction of Conditional Order by Execution** – The direction of the conditional order based on execution.
- **Activation on Partial Execution** – Partial execution of the order is considered. An "on-execution" order will be activated upon partial execution of the condition order.
- **Executed Volume** – Takes the executed volume of the order as the quantity for placing the stop order. The quantity of securities in an "on-execution" order is taken as the executed volume of the condition order.
- **Price of Linked Order** – The price of the linked limit order.
- **Withdrawal on Partial Execution** – Indicates the withdrawal of the stop order upon partial execution of the linked limit order.
- **Offset from Maximum** – The amount of offset from the maximum (minimum) price of the last transaction.
- **Protective Spread** – The size of the protective spread.
- **Take-Profit at Market Price** – Indicates if the "Take-Profit" order executes at the market price.

## Note

Working with orders is a low-level method of managing positions. For higher-level management, it is recommended to use the "Modify Position" component, described in [Modify Position](../positions/modify.md).

## See Also

[Modify Position](../positions/modify.md)