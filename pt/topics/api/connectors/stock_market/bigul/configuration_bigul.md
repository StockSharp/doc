# Configuração do conector: Bigul

Configure as propriedades a seguir antes de se conectar ao Bigul. A lista foi verificada com [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ClientCode` (`string`)
- `ApiKey` (`SecureString`)
- `ApiSecret` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `Token` (`SecureString`)
- `Source` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`BigulProducts`)
- `MarketProtection` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_bigul.md)

[Inicialização do adaptador](adapter_initialization_bigul.md)
