# Configuração do conector: Paytm Money

Configure as propriedades a seguir antes de se conectar ao Paytm Money. A lista foi verificada com [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ReadAccessToken` (`SecureString`)
- `PublicAccessToken` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RequestToken` (`SecureString`)
- `DefaultProduct` (`PaytmMoneyProducts`)
- `PortfolioName` (`string`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SecurityMasterFile` (`string`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_paytm_money.md)

[Inicialização do adaptador](adapter_initialization_paytm_money.md)
