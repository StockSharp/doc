# Configuração do conector: Birdeye

Configure as propriedades a seguir antes de se conectar ao Birdeye. A lista foi verificada com [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `WebSocketOrigin` (`string`)
- `Chain` (`string`)
- `TokenAddress` (`string`)
- `StreamingEnabled` (`bool`)
- `PriceInUsd` (`bool`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `MinimumLiquidity` (`decimal`)
- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_birdeye.md)

[Inicialização do adaptador](adapter_initialization_birdeye.md)
