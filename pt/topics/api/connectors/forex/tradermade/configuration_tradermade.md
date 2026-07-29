# Configuração do conector: TraderMade

Configure as propriedades a seguir antes de se conectar à TraderMade. A lista foi verificada com [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `RestKey` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam o acesso REST e WebSocket, os pontos de conexão, as opções de dados, os filtros de símbolos e os limites.

- `StreamingKey` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `EnableLadder` (`bool`)
- `Weekend` (`bool`)
- `QuoteCurrencies` (`string`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_tradermade.md)

[Inicialização do adaptador](adapter_initialization_tradermade.md)
