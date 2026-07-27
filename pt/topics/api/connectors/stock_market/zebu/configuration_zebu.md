# Configuração do conector: Zebu

Configure as propriedades a seguir antes de se conectar ao Zebu. A lista foi verificada com [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `UserId` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RefreshToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `TokenExpiresAt` (`DateTime?`)
- `DefaultProduct` (`ShoonyaProducts`)
- `ReconnectAttempts` (`int`)
- `AuthorizationAddress` (`Uri`)
- `RestEndpoint` (`string`)
- `InstrumentEndpointTemplate` (`string`)
- `WebSocketEndpoint` (`string`)

## Veja também

[Configuração gráfica](graphical_configuration_zebu.md)

[Inicialização do adaptador](adapter_initialization_zebu.md)
