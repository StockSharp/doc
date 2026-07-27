# Connector configuration: Bavest

Configure the following properties before connecting to Bavest. The list is verified against [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Currency` (`string`)
- `Exchange` (`string`)
- `ExchangeCode` (`string`)
- `FinancialFrequency` (`BavestFinancialFrequencies`)
- `TraceEtfMetrics` (`bool`)
- `ScreenerQuery` (`string`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `NewsLimit` (`int`)
- `DatasetLimit` (`int`)

## See also

[Graphical configuration](graphical_configuration_bavest.md)

[Adapter initialization](adapter_initialization_bavest.md)
