# Graphical configuration: OpenMarkets

For all StockSharp products, configure the connection in the [Connection settings window](../../../graphical_user_interface/connection_settings_window.md).

- `ClientId` - account or client identifier.
- `ClientSecret` - authentication credential.
- `AccountCode` - account or client identifier.
- `IsTest` - switch controlling connector behavior.
- `DataSource` - connection parameter. Default value: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - connection parameter. Default value: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - connection parameter. Default value: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - connection parameter.
- `OrderTaker` - connection parameter.
- `DefaultPriceMultiplier` - multiplier applied to received prices. Default value: `0.01m`.
- `DepthPollingInterval` - time interval. Default value: `TimeSpan.FromSeconds(2)`.

## See also

[Connectors](../../../connectors.md)

[Graphical configuration](../../graphical_configuration.md)

[Save and load settings](../../save_and_load_settings.md)

[Creating own connector](../../creating_own_connector.md)
