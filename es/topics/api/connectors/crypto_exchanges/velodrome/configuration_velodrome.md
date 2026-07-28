# Configuración del conector: Velodrome

Configure las siguientes propiedades antes de conectarse a Velodrome. La lista se ha verificado con [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `RpcEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Pools` (`string`)
- `HistoryBlockRange` (`int`)
- `HistoryBlockCount` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_velodrome.md)

[Inicialización del adaptador](adapter_initialization_velodrome.md)
