# Configuração do conector: SET Market Data

Configure as propriedades a seguir antes de se conectar ao SET Market Data. A lista foi verificada com [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Address` (`Uri`)
- `DataMode` (`SetMarketDataModes`)
- `Markets` (`string`)
- `IndexSectors` (`string`)
- `SecurityTypeCodes` (`string`)
- `IncludeOddLots` (`bool`)
- `IncludeIndices` (`bool`)

## Veja também

[Configuração gráfica](graphical_configuration_set_market_data.md)

[Inicialização do adaptador](adapter_initialization_set_market_data.md)
