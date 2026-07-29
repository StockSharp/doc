# Connector-Konfiguration: J-Quants

Konfigurieren Sie vor der Verbindung mit J-Quants die folgenden Adaptereigenschaften. Die Liste wurde anhand von [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Anbieterendpunkte, Anfrageabstände, Filter, Datenoptionen und Ergebnislimits.

- `RestEndpoint` (`string`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_jquants.md)

[Adapterinitialisierung](adapter_initialization_jquants.md)
