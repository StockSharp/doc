# Configuração do conector: Velora

Configure as propriedades a seguir antes de se conectar ao Velora. A lista foi verificada com [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Partner` (`string`)
- `Chain` (`VeloraChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Configurações avançadas

Estas propriedades controlam pontos de conexão, filtros, limites e outras opções específicas do provedor.

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Veja também

[Configuração gráfica](graphical_configuration_velora.md)

[Inicialização do adaptador](adapter_initialization_velora.md)
