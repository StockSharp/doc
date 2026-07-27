# Configuração do conector: Directa

Configure as propriedades a seguir antes de se conectar ao Directa. A lista foi verificada com [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Address` (`EndPoint`)
- `DataAddress` (`EndPoint`)
- `HistoryAddress` (`EndPoint`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `RequestTimeout` (`TimeSpan`)
- `AutoConfirmOrders` (`bool`)
- `MaxMarketDepth` (`int`)
- `TimeZoneId` (`string`)

## Veja também

[Configuração gráfica](graphical_configuration_directa.md)

[Inicialização do adaptador](adapter_initialization_directa.md)
