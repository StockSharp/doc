# Configuração do conector: J-Quants

Configure as propriedades a seguir antes de se conectar à J-Quants. A lista foi verificada com [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam os pontos de conexão, o ritmo das solicitações, os filtros, as opções de dados e os limites de resultados.

- `RestEndpoint` (`string`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)

## Veja também

[Configuração gráfica](graphical_configuration_jquants.md)

[Inicialização do adaptador](adapter_initialization_jquants.md)
