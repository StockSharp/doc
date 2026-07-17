# Connector-Konfiguration: OpenMarkets

Beziehen Sie die Zugangsdaten vom Anbieter und geben Sie die Verbindungsparameter an.

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
