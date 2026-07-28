# Configuração do conector: 0x

Configure as propriedades a seguir antes de se conectar ao 0x. A lista foi verificada com [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ApiKey` (`SecureString`)
- `Chain` (`ZeroXChains`)
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

[Configuração gráfica](graphical_configuration_zero_x.md)

[Inicialização do adaptador](adapter_initialization_zero_x.md)
