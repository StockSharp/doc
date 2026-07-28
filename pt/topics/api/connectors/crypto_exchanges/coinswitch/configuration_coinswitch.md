# Configuração do conector: CoinSwitch PRO

Configure as propriedades a seguir antes de se conectar ao CoinSwitch PRO. A lista foi verificada com [CoinSwitchMessageAdapter](xref:StockSharp.CoinSwitch.CoinSwitchMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoinSwitchProductTypes`)
- `SpotExchange` (`string`)
- `RestEndpoint` (`string`)
- `HftEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_coinswitch.md)

[Inicialização do adaptador](adapter_initialization_coinswitch.md)
