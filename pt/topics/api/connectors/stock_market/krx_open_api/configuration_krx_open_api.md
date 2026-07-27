# Configuração do conector: KRX Open API

Configure as propriedades a seguir antes de se conectar ao KRX Open API. A lista foi verificada com [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`KrxDataSets`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `MaxRequests` (`int`)
- `Address` (`Uri`)
- `SampleAddress` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_krx_open_api.md)

[Inicialização do adaptador](adapter_initialization_krx_open_api.md)
