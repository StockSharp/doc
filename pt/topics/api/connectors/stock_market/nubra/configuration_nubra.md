# Configuração do conector: Nubra

Configure as propriedades a seguir antes de se conectar ao Nubra. A lista foi verificada com [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `DeviceId` (`string`)
- `IsDemo` (`bool`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Phone` (`string`)
- `Mpin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NubraProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `UatRestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `UatMarketDataAddress` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_nubra.md)

[Inicialização do adaptador](adapter_initialization_nubra.md)
