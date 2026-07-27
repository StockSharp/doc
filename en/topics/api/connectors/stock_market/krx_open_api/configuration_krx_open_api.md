# Connector configuration: KRX Open API

Configure the following properties before connecting to KRX Open API. The list is verified against [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`KrxDataSets`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `MaxRequests` (`int`)
- `Address` (`Uri`)
- `SampleAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_krx_open_api.md)

[Adapter initialization](adapter_initialization_krx_open_api.md)
