# Configuração do conector: Quiver Quantitative

Configure as propriedades a seguir antes de se conectar ao Quiver Quantitative. A lista foi verificada com [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `LimitInsiderCodes` (`bool`)
- `MostRecentInstitutional` (`bool`)
- `IncludeNewFunds` (`bool`)
- `CorporateDonorCycle` (`string`)

## Veja também

[Configuração gráfica](graphical_configuration_quiver_quant.md)

[Inicialização do adaptador](adapter_initialization_quiver_quant.md)
