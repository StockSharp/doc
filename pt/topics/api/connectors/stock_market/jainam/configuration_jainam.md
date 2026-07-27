# Configuração do conector: Jainam

Configure as propriedades a seguir antes de se conectar ao Jainam. A lista foi verificada com [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `UserId` (`string`)
- `AppCode` (`string`)
- `ApiSecret` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`JainamProducts`)
- `ReconnectAttempts` (`int`)
- `PollingInterval` (`TimeSpan`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`string`)
- `WebSocketAddress` (`string`)

## Veja também

[Configuração gráfica](graphical_configuration_jainam.md)

[Inicialização do adaptador](adapter_initialization_jainam.md)
