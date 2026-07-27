# Configuração do conector: TPEx

Configure as propriedades a seguir antes de se conectar ao TPEx. A lista foi verificada com [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Address` (`Uri`)
- `Market` (`TpexMarkets`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `IncludeListedDerivatives` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)
- `MaxHistoryMonths` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_tpex.md)

[Inicialização do adaptador](adapter_initialization_tpex.md)
