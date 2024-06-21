# Interactive Brokers

**Interactive Brokers** \- trading platform to trade financial assets including stocks, options, futures, EFPs, futures options, forex, bonds, and funds.

Before you start writing trading robots for the current trading platform, it is recommended to read the links in the [Connectors](../../connectors.md). 

## Configuration Interactive Brokers

To work correctly with `Interactive Brokers` you should preset [TWS](https://interactivebrokers.com/en/index.php?f=1537) trading terminal. 

### Trader Workstation terminal configuration

1. You must allow connections from other programs (such as the trading algorithm on [S\#](../../../api.md)). To do this, open the settings menu "File \-\> Global configuration...". Select "Configuration \-\> API \-\> Settings" in the new window:

   ![ib settings](../../../../images/ib_settings.png)
2. Turn on "Enable ActiveX and Socket Clients" mode.
3. Also it is recommended to add the address of the computer that will run the algorithm (the local address \- 127.0.0.1). It allows not confirming the terminal connection permission each time you start the algorithm.

## Recommended content

[Connectors](../../connectors.md)

[Graphical configuration](../graphical_configuration.md)

[Save and load settings](../save_and_load_settings.md)

[Creating own connector](../creating_own_connector.md)

[Orders management](../../orders_management.md)

[Create new order](../../orders_management/create_new_order.md)

[Create new stop order](../../orders_management/create_new_stop_order.md)

[Adapter initialization Interactive Brokers](interactive_brokers/adapter_initialization_interactive_brokers.md)
