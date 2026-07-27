# Connector configuration: HDFC Securities

Configure the following properties before connecting to HDFC Securities. The list is verified against [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestToken` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Token` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`HdfcProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_hdfc_securities.md)

[Adapter initialization](adapter_initialization_hdfc_securities.md)
