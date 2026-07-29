# OpenFIGI

**OpenFIGI** connects StockSharp to OpenFIGI API v3 for global instrument identifier lookup.

## Key capabilities

- Source-verified capabilities: security search and filtered lookup by exchange, MIC, currency, market sector, and security type. The adapter returns instrument metadata and does not provide quotes or trading operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to map identifiers to FIGI records and build a normalized security catalogue.

Result coverage, rate limits, and anonymous or keyed access are controlled by OpenFIGI.

## See also

[Connector configuration](openfigi/configuration_openfigi.md)

[Graphical configuration](openfigi/graphical_configuration_openfigi.md)

[Adapter initialization](openfigi/adapter_initialization_openfigi.md)
