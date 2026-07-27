# Connector-Konfiguration: Euronext Web Services

Konfigurieren Sie vor der Verbindung mit Euronext Web Services die folgenden Adaptereigenschaften. Die Liste wurde anhand von [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `SessionQuality` (`EuronextSessionQualities`)
- `IntradayDepth` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_euronext_web_services.md)

[Adapterinitialisierung](adapter_initialization_euronext_web_services.md)
