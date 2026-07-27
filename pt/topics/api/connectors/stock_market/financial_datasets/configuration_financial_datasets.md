# Configuração do conector: Financial Datasets

Configure as propriedades a seguir antes de se conectar ao Financial Datasets. A lista foi verificada com [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `ActiveOnly` (`bool`)
- `FinancialPeriod` (`FinancialDatasetsPeriods`)
- `DataLimit` (`int`)
- `NewsLimit` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_financial_datasets.md)

[Inicialização do adaptador](adapter_initialization_financial_datasets.md)
