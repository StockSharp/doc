# Configuração do conector: Primary

Configure as propriedades a seguir antes de se conectar ao Primary. A lista foi verificada com [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Token` (`SecureString`)
- `Proprietary` (`string`)
- `DefaultMarket` (`string`)
- `MarketDataLevel` (`int`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SandboxWebSocketAddress` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_primary.md)

[Inicialização do adaptador](adapter_initialization_primary.md)
