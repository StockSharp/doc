# Configuração do conector: GuruFocus

Configure as propriedades a seguir antes de se conectar ao GuruFocus. A lista foi verificada com [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RegionCode` (`string`)
- `PageSize` (`int`)
- `MaxLookupPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `FilingFormType` (`string`)
- `GuruTradeActions` (`string`)

## Veja também

[Configuração gráfica](graphical_configuration_gurufocus.md)

[Inicialização do adaptador](adapter_initialization_gurufocus.md)
