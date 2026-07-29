# Configuração do conector: SimFin

Configure as propriedades a seguir antes de se conectar à SimFin. A lista foi verificada com [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam os pontos de conexão, o ritmo das solicitações, os filtros, as opções de dados e os limites de resultados.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `StatementTypes` (`string`)
- `Period` (`string`)
- `AsReported` (`bool`)
- `IncludeRatios` (`bool`)
- `MaximumRecords` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_simfin.md)

[Inicialização do adaptador](adapter_initialization_simfin.md)
