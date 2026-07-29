# SEC EDGAR

**SEC EDGAR** connects StockSharp to the official SEC EDGAR data and filing services.

## Key capabilities

- Source-verified capabilities: public-company lookup, filing news, historical submission files, and structured company facts through `SecEdgarDataTypes.CompanyFacts`. The adapter does not expose trading operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to discover SEC registrants and collect filings, filing news, and XBRL company facts for research.

Coverage and availability follow SEC EDGAR data, fair-access rules, and configured request limits.

## See also

[Connector configuration](sec_edgar/configuration_sec_edgar.md)

[Graphical configuration](sec_edgar/graphical_configuration_sec_edgar.md)

[Adapter initialization](sec_edgar/adapter_initialization_sec_edgar.md)
