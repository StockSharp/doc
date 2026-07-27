# Configuração do conector: Marketaux

Configure as propriedades a seguir antes de se conectar ao Marketaux. A lista foi verificada com [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Languages` (`string`)
- `EntityTypes` (`string`)
- `Countries` (`string`)
- `MustHaveEntities` (`bool`)
- `GroupSimilar` (`bool`)
- `NewsPageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `SentimentInterval` (`MarketauxIntervals`)

## Veja também

[Configuração gráfica](graphical_configuration_marketaux.md)

[Inicialização do adaptador](adapter_initialization_marketaux.md)
