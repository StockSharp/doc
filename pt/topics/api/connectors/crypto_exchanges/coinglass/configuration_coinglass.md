# Configuração do conector: CoinGlass

Configure as propriedades a seguir antes de se conectar ao CoinGlass. A lista foi verificada com [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinGlassMarketTypes`)
- `CandleMetric` (`CoinGlassCandleMetrics`)
- `Exchange` (`string`)
- `Symbol` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_coinglass.md)

[Inicialização do adaptador](adapter_initialization_coinglass.md)
