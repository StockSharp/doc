# Configuração do conector: EXANTE

Configure as propriedades a seguir antes de se conectar ao EXANTE. A lista foi verificada com [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `SummaryCurrency` (`string`)
- `PollingInterval` (`TimeSpan`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `MaxMarketDepth` (`int`)
- `HistoryRequestSize` (`int`)
- `LiveAddress` (`Uri`)
- `DemoAddress` (`Uri`)

## Veja também

[Configuração gráfica](graphical_configuration_exante.md)

[Inicialização do adaptador](adapter_initialization_exante.md)
