# Configuración del conector: Velora

Configure las siguientes propiedades antes de conectarse a Velora. La lista se ha verificado con [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Partner` (`string`)
- `Chain` (`VeloraChains`)
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

[Configuración gráfica](graphical_configuration_velora.md)

[Inicialización del adaptador](adapter_initialization_velora.md)
