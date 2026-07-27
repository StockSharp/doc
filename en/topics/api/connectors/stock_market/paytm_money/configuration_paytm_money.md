# Connector configuration: Paytm Money

Configure the following properties before connecting to Paytm Money. The list is verified against [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ReadAccessToken` (`SecureString`)
- `PublicAccessToken` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RequestToken` (`SecureString`)
- `DefaultProduct` (`PaytmMoneyProducts`)
- `PortfolioName` (`string`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SecurityMasterFile` (`string`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_paytm_money.md)

[Adapter initialization](adapter_initialization_paytm_money.md)
