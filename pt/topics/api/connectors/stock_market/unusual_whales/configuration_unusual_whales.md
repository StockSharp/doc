# Configuração do conector: Unusual Whales

Configure as propriedades a seguir antes de se conectar ao Unusual Whales. A lista foi verificada com [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `CandleLimit` (`int`)
- `NewsLimit` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `UnusualFlowOnly` (`bool`)
- `OtmMarketTide` (`bool`)
- `FiveMinuteMarketTide` (`bool`)

## Veja também

[Configuração gráfica](graphical_configuration_unusual_whales.md)

[Inicialização do adaptador](adapter_initialization_unusual_whales.md)
