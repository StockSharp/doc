# Configuração do conector: Settrade

Configure as propriedades a seguir antes de se conectar à Settrade. A lista foi verificada com [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AppCode` (`string`)
- `BrokerId` (`string`)
- `Account` (`string`)
- `Pin` (`SecureString`)
- `AccountType` (`SettradeAccountTypes`)
- `IsDemo` (`bool`)

## Configurações avançadas

Estas propriedades controlam os parâmetros de acesso, os pontos de conexão de produção e testes e as consultas periódicas do estado privado.

- `LoginParameters` (`string`)
- `RestEndpoint` (`string`)
- `DemoRestEndpoint` (`string`)
- `MarketDataEndpoint` (`string`)
- `DemoMarketDataEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_settrade.md)

[Inicialização do adaptador](adapter_initialization_settrade.md)
