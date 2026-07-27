# Configuração do conector: Tradernet

Configure as propriedades a seguir antes de se conectar ao Tradernet. A lista foi verificada com [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `PollingInterval` (`TimeSpan`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `MaxMarketDepth` (`int`)
- `SecuritiesPageSize` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_tradernet.md)

[Inicialização do adaptador](adapter_initialization_tradernet.md)
