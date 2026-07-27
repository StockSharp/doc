# Connector configuration: Financial Datasets

Configure the following properties before connecting to Financial Datasets. The list is verified against [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `ActiveOnly` (`bool`)
- `FinancialPeriod` (`FinancialDatasetsPeriods`)
- `DataLimit` (`int`)
- `NewsLimit` (`int`)

## See also

[Graphical configuration](graphical_configuration_financial_datasets.md)

[Adapter initialization](adapter_initialization_financial_datasets.md)
