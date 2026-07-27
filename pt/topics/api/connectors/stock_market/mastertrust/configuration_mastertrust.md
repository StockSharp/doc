# Configuração do conector: Mastertrust

Configure as propriedades a seguir antes de se conectar ao Mastertrust. A lista foi verificada com [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ClientId` (`string`)
- `OAuthClientId` (`string`)
- `OAuthClientSecret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `RedirectUri` (`Uri`)
- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`MastertrustProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_mastertrust.md)

[Inicialização do adaptador](adapter_initialization_mastertrust.md)
