# Connector configuration: B3 UP2DATA

Configure the following properties before connecting to B3 UP2DATA. The list is verified against [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `SasUri` (`SecureString`)
- `ChannelName` (`string`)
- `FileFormat` (`B3Up2DataFileFormats`)
- `BlobPrefix` (`string`)
- `LookbackDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `MaxRawFiles` (`int`)

## See also

[Graphical configuration](graphical_configuration_b3_up2data.md)

[Adapter initialization](adapter_initialization_b3_up2data.md)
