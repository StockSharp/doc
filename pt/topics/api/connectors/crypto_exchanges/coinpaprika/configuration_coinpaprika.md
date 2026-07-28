# Configuração do conector: CoinPaprika

Configure as propriedades a seguir antes de se conectar ao CoinPaprika. A lista foi verificada com [CoinPaprikaMessageAdapter](xref:StockSharp.CoinPaprika.CoinPaprikaMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `QuoteCurrency` (`string`)
- `ExchangeId` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_coinpaprika.md)

[Inicialização do adaptador](adapter_initialization_coinpaprika.md)
