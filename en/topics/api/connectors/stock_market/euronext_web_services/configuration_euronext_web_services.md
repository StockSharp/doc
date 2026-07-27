# Connector configuration: Euronext Web Services

Configure the following properties before connecting to Euronext Web Services. The list is verified against [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `SessionQuality` (`EuronextSessionQualities`)
- `IntradayDepth` (`int`)

## See also

[Graphical configuration](graphical_configuration_euronext_web_services.md)

[Adapter initialization](adapter_initialization_euronext_web_services.md)
