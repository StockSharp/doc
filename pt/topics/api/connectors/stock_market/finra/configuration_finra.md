# Configuração do conector: FINRA

Configure as propriedades a seguir antes de se conectar ao FINRA. A lista foi verificada com [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`FinraDataSets`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Token` (`SecureString`)
- `WeeklyTierIdentifier` (`string`)
- `WeeklySummaryTypeCode` (`string`)
- `PageSize` (`int`)
- `MaxRecords` (`int`)
- `DataVersion` (`int`)
- `Address` (`Uri`)
- `AuthAddress` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_finra.md)

[Inicialização do adaptador](adapter_initialization_finra.md)
