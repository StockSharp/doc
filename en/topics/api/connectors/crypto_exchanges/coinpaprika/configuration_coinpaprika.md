# Connector configuration: CoinPaprika

Configure the following properties before connecting to CoinPaprika. The list is verified against [CoinPaprikaMessageAdapter](xref:StockSharp.CoinPaprika.CoinPaprikaMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `QuoteCurrency` (`string`)
- `ExchangeId` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## See also

[Graphical configuration](graphical_configuration_coinpaprika.md)

[Adapter initialization](adapter_initialization_coinpaprika.md)
