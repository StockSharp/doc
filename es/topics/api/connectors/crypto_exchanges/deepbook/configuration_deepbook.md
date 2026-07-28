# Configuración del conector: DeepBook

Configure las siguientes propiedades antes de conectarse a DeepBook. La lista se ha verificado con [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `IndexerEndpoint` (`string`)
- `GrpcEndpoint` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PackageId` (`string`)
- `ClockObjectId` (`string`)
- `Pools` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLimit` (`int`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_deepbook.md)

[Inicialización del adaptador](adapter_initialization_deepbook.md)
