# Configuração do conector: MarketData.app

Configure as propriedades a seguir antes de se conectar à MarketData.app. A lista foi verificada com [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `RestEndpoint` (`Uri`)

## Configurações avançadas

Estas propriedades controlam os pontos de conexão, o ritmo das solicitações, os filtros, as opções de dados e os limites de resultados.

- `ExtendedHours` (`bool`)
- `AdjustSplits` (`bool`)
- `MaximumOptionContracts` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_marketdataapp.md)

[Inicialização do adaptador](adapter_initialization_marketdataapp.md)
