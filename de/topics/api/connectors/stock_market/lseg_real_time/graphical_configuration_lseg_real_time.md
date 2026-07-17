# Grafische Konfiguration: LSEG Real-Time

In allen StockSharp-Produkten wird die Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) konfiguriert.

- `AuthenticationMode` - Betriebsart oder Option des Konnektors.
- `Address` - Adresse des Dienstes.
- `StandbyAddress` - Adresse des Dienstes.
- `IsHotStandby` - Schalter für das Verhalten des Konnektors.
- `Login` - Konto- oder Clientkennung.
- `Password` - Zugangsdaten für die Authentifizierung.
- `ClientId` - Konto- oder Clientkennung.
- `Secret` - Zugangsdaten für die Authentifizierung.
- `ApplicationId` - Konto- oder Clientkennung. Standardwert: `256`.
- `Service` - Verbindungsparameter. Standardwert: `ELEKTRON_DD`.
- `Region` - Verbindungsparameter. Standardwert: `us-east-1`.
- `Position` - Verbindungsparameter.
- `Scope` - Verbindungsparameter. Standardwert: `trapi.streaming.pricing.read`.
- `AuthUrl` - Adresse des Dienstes.
- `DiscoveryUrl` - Adresse des Dienstes. Standardwert: `https://api.refinitiv.com/streaming/pricing/v1/`.

## Siehe auch

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)

[Eigenen Konnektor erstellen](../../creating_own_connector.md)
