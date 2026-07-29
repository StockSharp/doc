# Configuração do conector: Finage

Configure as propriedades a seguir antes de se conectar à Finage. A lista foi verificada com [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ApiKey` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam o acesso REST e WebSocket, os pontos de conexão, as opções de dados, os filtros de símbolos e os limites.

- `StreamingToken` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_finage.md)

[Inicialização do adaptador](adapter_initialization_finage.md)
