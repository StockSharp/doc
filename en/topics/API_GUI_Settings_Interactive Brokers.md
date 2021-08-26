# Graphical configuration Interactive Brokers

For all [S\#](StockSharpAbout.md) products, graphical configuration of the connection is performed on the [Connection settings window](API_UI_ConnectorWindow.md):

![API GUI Settings Interactive Brokers](../images/API_GUI_Settings_Interactive Brokers.png)

- **Address**

   \- TWS address.
- **Identifier**

   \- Unique ID. Used when several clients are connected to one terminal or gateway.
- **Real\-time**

   \- Should real\-time or 'frozen' on broker server data be used.
- **Logging level**

   \- Server messages logging level.
- **Market data fields**

   \- Market data fields, which will be received with subscribed to Levell messages.
- **Protocol**

   \- SSL protocol to establish connect
- **Certificate**

   \- SSL certificate.
- **Password**

   \- SSL certificate password.
- **Check revocation**

   \- Check certificate revocation.
- **Validate remote**

   \- Validate remove certificates.
- **Host name**

   \- The name of the server that shares SSL connection.
- **MaxVersion**

   \- MaxVersion
- **Heart beat**

   \- Server check interval for track the connection alive. By default equal to 1 minute.
- **Reconnection settings**

   \- Mechanism for tracking connections with the trading system settings. (

  [Reconnection settings](Reconnect.md)

  )

## Recommended content

[Connectors](API_Connectors.md)

[Graphical configuration](API_ConnectorsUIConfiguration.md)

[Creating own connector](ConnectorCreating.md)

[Save and load settings](API_Connectors_SaveConnectorSettings.md)
