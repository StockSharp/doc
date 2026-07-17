# Grafische Konfiguration: Databento

In allen StockSharp-Produkten wird die Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) konfiguriert.

- `Key` - Zugangsdaten für die Authentifizierung.
- `Dataset` - Verbindungsparameter. Standardwert: `GLBX.MDP3`.
- `LiveAddress` - Adresse des Dienstes.
- `HistoricalAddress` - Adresse des Dienstes. Standardwert: `https://hist.databento.com/v0/timeseries.get_range`.
- `Symbology` - Betriebsart oder Option des Konnektors. Standardwert: `DatabentoSymbologyTypes.RawSymbol`.

## Siehe auch

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)

[Eigenen Konnektor erstellen](../../creating_own_connector.md)
