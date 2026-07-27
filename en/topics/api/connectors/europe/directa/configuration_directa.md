# Connector configuration: Directa

Configure the following properties before connecting to Directa. The list is verified against [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Address` (`EndPoint`)
- `DataAddress` (`EndPoint`)
- `HistoryAddress` (`EndPoint`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RequestTimeout` (`TimeSpan`)
- `AutoConfirmOrders` (`bool`)
- `MaxMarketDepth` (`int`)
- `TimeZoneId` (`string`)

## See also

[Graphical configuration](graphical_configuration_directa.md)

[Adapter initialization](adapter_initialization_directa.md)
