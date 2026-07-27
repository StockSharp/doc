# Configuração do conector: BCS

Configure as propriedades a seguir antes de se conectar ao BCS. A lista foi verificada com [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `IsReadOnly` (`bool`)
- `PortfolioName` (`string`)
- `PollingInterval` (`TimeSpan`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Veja também

[Configuração gráfica](graphical_configuration_bcs.md)

[Inicialização do adaptador](adapter_initialization_bcs.md)
