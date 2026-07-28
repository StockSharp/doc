# Configuração do conector: Velodrome

Configure as propriedades a seguir antes de se conectar ao Velodrome. A lista foi verificada com [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `RpcEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Pools` (`string`)
- `HistoryBlockRange` (`int`)
- `HistoryBlockCount` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_velodrome.md)

[Inicialização do adaptador](adapter_initialization_velodrome.md)
