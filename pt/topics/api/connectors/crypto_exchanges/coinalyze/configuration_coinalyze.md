# Configuração do conector: Coinalyze

Configure as propriedades a seguir antes de se conectar ao Coinalyze. A lista foi verificada com [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinalyzeMarketTypes`)
- `CandleMetric` (`CoinalyzeCandleMetrics`)
- `Exchange` (`string`)
- `ConvertToUsd` (`bool`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RequestInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_coinalyze.md)

[Inicialização do adaptador](adapter_initialization_coinalyze.md)
