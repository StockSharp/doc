# Configuração do conector: SEC EDGAR

Configure as propriedades a seguir antes de se conectar à SEC EDGAR. A lista foi verificada com [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `DataEndpoint` (`Uri`)
- `UserAgent` (`string`)

## Configurações avançadas

Estas propriedades controlam os pontos de conexão, o ritmo das solicitações, os filtros, as opções de dados e os limites de resultados.

- `WebsiteEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Forms` (`string`)
- `MaximumHistoricalFiles` (`int`)
- `MaximumFacts` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_sec_edgar.md)

[Inicialização do adaptador](adapter_initialization_sec_edgar.md)
