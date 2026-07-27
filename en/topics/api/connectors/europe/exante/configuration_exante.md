# Connector configuration: EXANTE

Configure the following properties before connecting to EXANTE. The list is verified against [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `SummaryCurrency` (`string`)
- `PollingInterval` (`TimeSpan`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `MaxMarketDepth` (`int`)
- `HistoryRequestSize` (`int`)
- `LiveAddress` (`Uri`)
- `DemoAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_exante.md)

[Adapter initialization](adapter_initialization_exante.md)
