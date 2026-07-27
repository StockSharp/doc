# Configuração do conector: Rupeezy

Configure as propriedades a seguir antes de se conectar ao Rupeezy. A lista foi verificada com [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ApplicationId` (`string`)
- `ApiKey` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`RupeezyProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_rupeezy.md)

[Inicialização do adaptador](adapter_initialization_rupeezy.md)
