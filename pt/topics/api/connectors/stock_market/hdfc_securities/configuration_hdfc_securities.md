# Configuração do conector: HDFC Securities

Configure as propriedades a seguir antes de se conectar ao HDFC Securities. A lista foi verificada com [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestToken` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Token` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`HdfcProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_hdfc_securities.md)

[Inicialização do adaptador](adapter_initialization_hdfc_securities.md)
