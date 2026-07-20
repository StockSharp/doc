# Grafische Konfiguration: Handelstechnologien

In allen StockSharp-Produkten wird die Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) konfiguriert.

- `SdkPath` - Pfad zu einer lokalen Datei oder einem Verzeichnis.
- `AppSecretKey` - Zugangsdaten für die Authentifizierung.
- `Environment` - Betriebsart oder Option des Konnektors. Standardwert: `TradingTechnologiesEnvironments.ProdSim`.
- `InitializationTimeout` - Zeitintervall. Standardwert: `5000`.
- `MarketDepth` - Anzahl der abzufragenden Orderbuchtiefen. Standardwert: `20`.
- `IsBinaryProtocol` - Schalter für das Verhalten des Konnektors. Standardwert: `true`.
- `IsOptionsEnabled` - Schalter für das Verhalten des Konnektors. Standardwert: `true`.

## Siehe auch

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)

[Eigenen Konnektor erstellen](../../creating_own_connector.md)
