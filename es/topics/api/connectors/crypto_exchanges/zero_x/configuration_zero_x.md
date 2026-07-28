# Configuración del conector: 0x

Configure las siguientes propiedades antes de conectarse a 0x. La lista se ha verificado con [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ApiKey` (`SecureString`)
- `Chain` (`ZeroXChains`)
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
- `IsAutoApprove` (`bool`)

## Véase también

[Configuración gráfica](graphical_configuration_zero_x.md)

[Inicialización del adaptador](adapter_initialization_zero_x.md)
