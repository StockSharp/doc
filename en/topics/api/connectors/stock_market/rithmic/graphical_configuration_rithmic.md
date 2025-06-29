# Graphical configuration Rithmic

For all [S\#](../../../../api.md) products, graphical configuration of the connection is performed on the [Connection settings window](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings Rithmic](../../../../../images/api_gui_settings_rithmic.png)

- **Login** \- Login.
- **Password** \- Password.
- **Certificate** \- Path to certificate file, necessary to connect to Rithmic system.
- **File log** \- Path to log file.
- **Server type** \- Server type.
- **Point (admin)** \- Connection point for administrative functions (initialization\/deinitialization).
- **Point (data)** \- Connection point to market data.
- **Login (trans)** \- Additional login. Used when transaction sending is carried out to a separate server.
- **Point (transactions)** \- Connection point to the transactions execution system.
- **Password (trans)** \- Additional password. Used when transaction sending is carried out to a separate server.
- **Point (positions)** \- Connection point for access to portfolios and positions information.
- **Point (history)** \- Connection point for access to history data.
- **Domain (address)** \- Domain address.
- **Domain (name)** \- Domain name.
- **Licenses** \- Licenses server address.
- **Broker** \- Broker address.
- **Log (address)** \- Logger address.
- **User name (hist)** \- Additional login. User id used for authentication with the history plant.
- **Password (hist)** \- Additional password. Password used for authentication with the history plant
- **Heart beat** \- Server check interval to track that the connection is alive. By default equal to 1 minute.
- **Reconnection settings** \- Mechanism for tracking connections with the trading system settings. ([Reconnection settings](../../reconnection_settings.md))

## Recommended content

[Connectors](../../../connectors.md)

[Graphical configuration](../../graphical_configuration.md)

[Creating own connector](../../creating_own_connector.md)

[Save and load settings](../../save_and_load_settings.md)
