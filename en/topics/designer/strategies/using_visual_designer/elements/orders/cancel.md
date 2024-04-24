# Order Cancellation

![Designer Cancellations 00](../../../../../../images/designer_cancellations_00.png)

This block is used to cancel an order for an instrument.

### Incoming Sockets

Incoming Sockets

- **Trigger** – the event that triggers the order cancellation.
- **Order** – the signal used to determine the moment when an order needs to be canceled.

### Outgoing Sockets

Outgoing Sockets

- **Order** – the canceled order, which can be used to retrieve transactions for it using the **Transactions** element, as well as to display it on the chart using the **Chart Panel** block.
- **Error** – an error in cancelling the order (for example, the order has already been executed or canceled earlier).

## See Also

[Mass Orders Cancel](mass_cancel.md)