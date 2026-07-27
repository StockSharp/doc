# Connector-Konfiguration: Financial Datasets

Konfigurieren Sie vor der Verbindung mit Financial Datasets die folgenden Adaptereigenschaften. Die Liste wurde anhand von [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `ActiveOnly` (`bool`)
- `FinancialPeriod` (`FinancialDatasetsPeriods`)
- `DataLimit` (`int`)
- `NewsLimit` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_financial_datasets.md)

[Adapterinitialisierung](adapter_initialization_financial_datasets.md)
