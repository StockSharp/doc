# Trades by strategy

![Designer The transaction strategy 00](../../../../../../images/designer_trades_strategy_00.png)

The cube is used to obtain all the strategy trades. 

### Incoming sockets

Incoming sockets

- **Instrument** \- the instrument for which you need to obtain trades. If the instrument is not passed, then trades by all strategy instruments are transferred to the output.

### Outgoing sockets

Outgoing sockets

- **Trades** \- trades arising from the passed instrument. They can be used both for displaying on the chart using the **Chart panel** element, and for position protection using the **Position protection** element.

## Recommended content

[Notification](../notifying/notification.md)
