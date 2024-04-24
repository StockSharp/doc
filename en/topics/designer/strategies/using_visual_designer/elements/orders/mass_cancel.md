# Mass Order Cancellations

![Designer Mass Cancellations 00](../../../../../../images/designer_mass_cancellations_00.png)

This block is used for canceling all orders for an instrument.

### Incoming Sockets

Incoming Sockets

- **Trigger** - the signal that determines the moment when it's necessary to cancel orders.
- **Portfolio** – the portfolio for which all orders need to be canceled.
- **Security** – the instrument for which all orders need to be canceled.

### Outgoing Sockets

Outgoing Sockets

- **Result** - a flag signaling the success of the operation.

### Parameters

Parameters

- **Direction** – the direction of the orders being canceled (buy or sell), acts as a cancellation signal for the order.

## See Also

[Order Registration](register.md)
