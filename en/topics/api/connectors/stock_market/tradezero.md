# TradeZero

**TradeZero** is a broker for trading US equities and single-leg options.

The connector supports order registration, cancellation and cancel-and-replace, account data, positions, current orders and P&L. Account updates are streamed over WebSocket. Quotes, market depth and candles are returned as finite REST snapshots because TradeZero does not provide a public market-data WebSocket.

Before developing trading robots for TradeZero, review the links in the [Connectors](../../connectors.md) section.

## Recommended content

[Connectors](../../connectors.md)

[Graphical configuration](../graphical_configuration.md)

[Save and load settings](../save_and_load_settings.md)

[Creating own connector](../creating_own_connector.md)

[Orders management](../../orders_management.md)

[Create new order](../../orders_management/create_new_order.md)

[Create new stop order](../../orders_management/create_new_stop_order.md)
