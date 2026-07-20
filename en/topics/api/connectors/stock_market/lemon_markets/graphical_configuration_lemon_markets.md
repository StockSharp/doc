# Graphical configuration: lemon.markets

For all StockSharp products, configure the connection in the [Connection settings window](../../../graphical_user_interface/connection_settings_window.md).

- `ApiKey` - authentication credential.
- `IsDemo` - switch controlling connector behavior. Default value: `true`.
- `AccountId` - account or client identifier.
- `SecuritiesAccountId` - account or client identifier.
- `DataPrivacyPrincipal` - connection parameter.
- `DataPrivacyJustification` - connection parameter. Default value: `app_usage-stocksharp`.
- `PersonId` - account or client identifier.
- `DefaultFeeAmount` - fee amount used when the service does not provide one.
- `IsAppropriatenessConsentAccepted` - switch controlling connector behavior.
- `PollingInterval` - time interval. Default value: `TimeSpan.FromSeconds(10)`.

## See also

[Connectors](../../../connectors.md)

[Graphical configuration](../../graphical_configuration.md)

[Save and load settings](../../save_and_load_settings.md)

[Creating own connector](../../creating_own_connector.md)
