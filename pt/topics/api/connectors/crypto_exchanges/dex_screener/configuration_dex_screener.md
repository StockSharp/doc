# Configuração do conector: DEX Screener

Configure as propriedades a seguir antes de se conectar ao DEX Screener. A lista foi verificada com [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `RestEndpoint` (`string`)
- `ChainId` (`string`)
- `TokenAddress` (`string`)
- `SearchQuery` (`string`)
- `PriceInUsd` (`bool`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_dex_screener.md)

[Inicialização do adaptador](adapter_initialization_dex_screener.md)
