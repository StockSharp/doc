# DukasCopy JForex

**DukasCopy JForex** connects StockSharp to Dukascopy Bank through the official **Java JForex SDK**.

The JForex SDK establishes the secure authenticated connection to Dukascopy trading servers. The StockSharp .NET adapter communicates with the SDK through a TCP bridge bound only to the local loopback interface. Dukascopy does not publish a supported native .NET transport, so the connector does not reproduce the broker's internal protocol.

The connector supports instrument lookup, Level 1 quotes, ticks, order books, candles and historical data, together with portfolio information and order registration, replacement and cancellation.

Java is required. Build the included Maven bridge into an executable JAR and set `BridgeJarPath`, or run the bridge separately and configure `BridgePort`. Demo and live JForex service addresses are exposed as adapter settings.

Before developing trading robots, review the materials in the [Connectors](../../connectors.md) section.

## See also

[Connectors](../../connectors.md)

[Graphical configuration](../graphical_configuration.md)

[Save and load settings](../save_and_load_settings.md)

[Creating own connector](../creating_own_connector.md)

[Orders management](../../orders_management.md)

[Create new order](../../orders_management/create_new_order.md)

[Create new stop order](../../orders_management/create_new_stop_order.md)
