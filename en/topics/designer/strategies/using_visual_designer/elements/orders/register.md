# Order Registration

![Designer Position opening 00](../../../../../../images/designer_position_opening_00.png)

This block is used for placing an order for an instrument.

### Incoming Sockets

Incoming Sockets

- **Security** – the instrument for which an order needs to be placed.
- **Price** – the numerical price value if placing a non-market order.
- **Trigger** - the signal that determines when to place an order.
- **Volume** – the numerical volume value.
- **Portfolio** – the portfolio for which an order needs to be placed.

### Outgoing Sockets

Outgoing Sockets

- **Order** – the placed order, which can be used to obtain trades for it using the **Transactions by Order** element and for display on the chart using the **Chart Panel** block.
- **Error** – an error in placing the order.
- **Trade** – the trade for the placed order.

### Parameters

Parameters

- **Direction** – the direction of the order (buy or sell), acts as a signal for placing the order.
- **Market** – indicates a market order.
- **Position Condition** – an additional condition checking the possibility of placing an order. For example, under the condition **Close Position**, placing orders that increase the position is prohibited.
- **Zero Price** – a zero price registers a market order.
- **Lifetime** – the lifetime of a limit order.

## Conditional Order Settings

**Conditional Order** — an order with additional conditions that determine the moment of placement in the trading system depending on the current market situation.

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Connection** \- the connection where the order will be registered.
- **Stop\-order type** \- the stop\-order type.
- **Result** \- the result of the stop order execution.
- **Security ID** \- the instrument ID for s stop\-orders with a condition for another instrument.
- **Stop price condition** \- the stop\-price condition. Used for orders such as "Stop\-price for another security".
- **Stop\-price** \- the stop\-price that sets the condition for the stop\-order to trigger.
- **Stop\-limit price** \- the stop\-limit price. It is similar to the stop\-price, but it is used only for the order type "Take\-profit and Stop\-limit".
- **Stop\-limit at market price** \- the attribute of execution of the "Stop\-limit" order at the market price.
- **Conditions checking interval** \- time for order conditions checking only for a specified time period (if the value is null, then do not check). It is used for orders of the types "Take\-profit and stop\-limit" and "Take\-profit and stop\-limit on order".
- **On execution conditional order ID** \- the ID of the conditional order on execution.
- **On filled conditional order direction** \- the direction of the conditional order on execution.
- **Activation in case of partial execution** \- a partial execution of the order is taken into account. The "on execution" order will be activated when the order\-condition is partially executed.
- **Filled volume** \- to take the executed volume of the order as the amount of the registered stop\-order. As the number of securities in the "on execution" order the executed volume of the order\-condition is accepted.
- **Price of linked order** \- the price of the linked limited order.
- **Cancellation when partially filled** \- the attribute of the stop\-order cancellation in case of partial execution of the linked limited order.
- **Offset from maximum** \- the amount of an offset from the maximum (minimum) of the last trade price.
- **Protection spread** \- the amount of the protection spread.
- **Take\-profit at market price** \- the attribute of execution of the "Take\-profit" order at the market price.

## Recommended content

[Order Movement](modify.md)