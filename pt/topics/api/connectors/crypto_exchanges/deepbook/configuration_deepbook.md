# Configuração do conector: DeepBook

Configure as propriedades a seguir antes de se conectar ao DeepBook. A lista foi verificada com [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `IndexerEndpoint` (`string`)
- `GrpcEndpoint` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `PackageId` (`string`)
- `ClockObjectId` (`string`)
- `Pools` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLimit` (`int`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_deepbook.md)

[Inicialização do adaptador](adapter_initialization_deepbook.md)
