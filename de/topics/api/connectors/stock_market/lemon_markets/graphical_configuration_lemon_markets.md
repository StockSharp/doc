# Grafische Konfiguration: lemon.markets

In allen StockSharp-Produkten wird die Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) konfiguriert.

- `ApiKey` - Zugangsdaten für die Authentifizierung.
- `IsDemo` - Schalter für das Verhalten des Konnektors. Standardwert: `true`.
- `AccountId` - Konto- oder Clientkennung.
- `SecuritiesAccountId` - Konto- oder Clientkennung.
- `DataPrivacyPrincipal` - Verbindungsparameter.
- `DataPrivacyJustification` - Verbindungsparameter. Standardwert: `app_usage-stocksharp`.
- `PersonId` - Konto- oder Clientkennung.
- `DefaultFeeAmount` - Gebührenbetrag, wenn der Dienst keinen Wert liefert.
- `IsAppropriatenessConsentAccepted` - Schalter für das Verhalten des Konnektors.
- `PollingInterval` - Zeitintervall. Standardwert: `TimeSpan.FromSeconds(10)`.

## Siehe auch

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)

[Eigenen Konnektor erstellen](../../creating_own_connector.md)
