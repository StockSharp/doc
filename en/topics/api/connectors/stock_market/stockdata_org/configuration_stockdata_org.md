# Connector configuration: StockData.org

Configure the following properties before connecting to StockData.org. The list is verified against [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `ExtendedHours` (`bool`)
- `AdjustedIntraday` (`bool`)
- `NewsLanguage` (`string`)
- `NewsPageSize` (`int`)
- `MaxRequests` (`int`)
- `QuoteTimeZoneId` (`string`)

## See also

[Graphical configuration](graphical_configuration_stockdata_org.md)

[Adapter initialization](adapter_initialization_stockdata_org.md)
