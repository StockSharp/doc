# Grafische Konfiguration: Bloomberg BLPAPI and EMSX

In allen StockSharp-Produkten wird die Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) konfiguriert.

- `ServerAddress` - Adresse des Dienstes. Standardwert: `new DnsEndPoint("localhost", 8194)`.
- `SdkPath` - Pfad zu einer lokalen Datei oder einem Verzeichnis.
- `IsEmsxEnabled` - Schalter für das Verhalten des Konnektors.
- `EmsxService` - Verbindungsparameter. Standardwert: `//blp/emapisvc`.
- `Broker` - Verbindungsparameter.

## Siehe auch

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)

[Eigenen Konnektor erstellen](../../creating_own_connector.md)
