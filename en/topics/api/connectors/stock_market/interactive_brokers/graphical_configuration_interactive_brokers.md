# Graphical configuration Interactive Brokers

For all [S\#](../../../../api.md) products, graphical configuration of the connection is performed on the [Connection settings window](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings Interactive Brokers](../../../../../images/api_gui_settings_interactivebrokers.png)

- **Address** \- TWS address.
- **Identifier** \- Unique ID. Used when several clients are connected to one terminal or gateway.
- **Real\-time** \- Should real\-time or 'frozen' on broker server data be used.
- **Logging level** \- Server messages logging level.
- **Market data fields** \- Market data fields, which will be received with subscribed to Level1 messages.
- **Protocol** \- SSL protocol to establish connection
- **Certificate** \- SSL certificate.
- **Password** \- SSL certificate password.
- **Check revocation** \- Check certificate revocation.
- **Validate remote** \- Validate remote certificates.
- **Host name** \- The name of the server that shares SSL connection.
- **MaxVersion** \- MaxVersion
- **Heart beat** \- Server check interval for tracking the connection alive. By default equal to 1 minute.
- **Reconnection settings** \- Mechanism for tracking connections with the trading system settings. ([Reconnection settings](../../reconnection_settings.md))

## Recommended content

[Connectors](../../../connectors.md)

[Graphical configuration](../../graphical_configuration.md)

[Creating own connector](../../creating_own_connector.md)

[Save and load settings](../../save_and_load_settings.md)
