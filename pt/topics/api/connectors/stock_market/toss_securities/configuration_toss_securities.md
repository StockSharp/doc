# Configuração do conector: Toss Securities

Configure as propriedades a seguir antes de se conectar ao Toss Securities. A lista foi verificada com [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AccountSequence` (`long`)
- `PollingInterval` (`TimeSpan`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PortfolioName` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `AdjustedCandles` (`bool`)
- `RestAddress` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_toss_securities.md)

[Inicialização do adaptador](adapter_initialization_toss_securities.md)
