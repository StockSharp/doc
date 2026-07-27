# Graphical configuration: DukasCopy JForex

For all StockSharp products, configure the connection in the [Connection settings window](../../../graphical_user_interface/connection_settings_window.md).

- `Login` - account or client identifier.
- `Password` - authentication credential.
- `IsDemo` - switch controlling connector behavior. Default value: `true`.
- `DemoAddress` - JForex JNLP service address used in demo mode.
- `LiveAddress` - JForex JNLP service address used in live mode.
- `BridgePort` - TCP port of the local bridge. Default value: `27431`.
- `BridgeJarPath` - optional path to the executable bridge JAR.

When `BridgeJarPath` is empty, start the Java bridge separately on the configured local port.

## See also

[Connectors](../../../connectors.md)

[Graphical configuration](../../graphical_configuration.md)

[Save and load settings](../../save_and_load_settings.md)

[Creating own connector](../../creating_own_connector.md)
