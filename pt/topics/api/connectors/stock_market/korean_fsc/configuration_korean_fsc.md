# Configuração do conector: Korean FSC

Configure as propriedades a seguir antes de se conectar ao Korean FSC. A lista foi verificada com [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `DataSet` (`KoreanFscDataSets`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Address` (`Uri`)
- `Market` (`KoreanFscMarkets`)
- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_korean_fsc.md)

[Inicialização do adaptador](adapter_initialization_korean_fsc.md)
