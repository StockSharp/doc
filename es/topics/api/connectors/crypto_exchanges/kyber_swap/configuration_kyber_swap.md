# Configuración del conector: KyberSwap

Configure las siguientes propiedades antes de conectarse a KyberSwap. La lista se ha verificado con [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ClientId` (`string`)
- `Chain` (`KyberSwapChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `TransactionLifetime` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Véase también

[Configuración gráfica](graphical_configuration_kyber_swap.md)

[Inicialización del adaptador](adapter_initialization_kyber_swap.md)
