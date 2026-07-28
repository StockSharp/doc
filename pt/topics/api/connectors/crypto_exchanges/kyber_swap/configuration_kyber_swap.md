# Configuração do conector: KyberSwap

Configure as propriedades a seguir antes de se conectar ao KyberSwap. A lista foi verificada com [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `ClientId` (`string`)
- `Chain` (`KyberSwapChains`)
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
- `TransactionLifetime` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Veja também

[Configuração gráfica](graphical_configuration_kyber_swap.md)

[Inicialização do adaptador](adapter_initialization_kyber_swap.md)
