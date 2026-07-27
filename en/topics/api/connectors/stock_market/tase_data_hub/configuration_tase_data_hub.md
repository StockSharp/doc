# Connector configuration: TASE Data Hub

Configure the following properties before connecting to TASE Data Hub. The list is verified against [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `Scope` (`string`)
- `SecurityLookupDays` (`int`)
- `ReferenceCacheTimeout` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_tase_data_hub.md)

[Adapter initialization](adapter_initialization_tase_data_hub.md)
