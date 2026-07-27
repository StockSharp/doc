# Configuração do conector: Firstock

Configure as propriedades a seguir antes de se conectar ao Firstock. A lista foi verificada com [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `UserId` (`string`)
- `Password` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `VendorCode` (`string`)
- `ApiKey` (`SecureString`)
- `Token` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`FirstockProducts`)
- `MarketProtection` (`decimal`)
- `PriceDivisor` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `SymbolsAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_firstock.md)

[Inicialização do adaptador](adapter_initialization_firstock.md)
