# Configuração do conector: OpenFIGI

Configure as propriedades a seguir antes de se conectar à OpenFIGI. A lista foi verificada com [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam os pontos de conexão, o ritmo das solicitações, os filtros, as opções de dados e os limites de resultados.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)
- `MaximumResults` (`int`)
- `ExchangeCode` (`string`)
- `MicCode` (`string`)
- `Currency` (`string`)
- `MarketSector` (`string`)
- `SecurityType2` (`string`)
- `IncludeUnlistedEquities` (`bool`)

## Veja também

[Configuração gráfica](graphical_configuration_openfigi.md)

[Inicialização do adaptador](adapter_initialization_openfigi.md)
