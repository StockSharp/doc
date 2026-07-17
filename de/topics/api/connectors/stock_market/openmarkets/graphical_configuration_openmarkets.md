# Grafische Konfiguration: OpenMarkets

In allen StockSharp-Produkten wird die Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) konfiguriert.

- `ClientId` - Konto- oder Clientkennung.
- `ClientSecret` - Zugangsdaten für die Authentifizierung.
- `AccountCode` - Konto- oder Clientkennung.
- `IsTest` - Schalter für das Verhalten des Konnektors.
- `DataSource` - Verbindungsparameter. Standardwert: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - Verbindungsparameter. Standardwert: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - Verbindungsparameter. Standardwert: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - Verbindungsparameter.
- `OrderTaker` - Verbindungsparameter.
- `DefaultPriceMultiplier` - numerischer Konnektorparameter. Standardwert: `0.01m`.
- `DepthPollingInterval` - Zeitintervall. Standardwert: `TimeSpan.FromSeconds(2)`.

## Siehe auch

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)

[Eigenen Konnektor erstellen](../../creating_own_connector.md)
