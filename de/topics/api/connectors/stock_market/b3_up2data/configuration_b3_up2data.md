# Connector-Konfiguration: B3 UP2DATA

Konfigurieren Sie vor der Verbindung mit B3 UP2DATA die folgenden Adaptereigenschaften. Die Liste wurde anhand von [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `SasUri` (`SecureString`)
- `ChannelName` (`string`)
- `FileFormat` (`B3Up2DataFileFormats`)
- `BlobPrefix` (`string`)
- `LookbackDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `MaxRawFiles` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_b3_up2data.md)

[Adapterinitialisierung](adapter_initialization_b3_up2data.md)
