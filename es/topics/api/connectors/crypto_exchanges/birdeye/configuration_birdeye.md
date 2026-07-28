# Configuración del conector: Birdeye

Configure las siguientes propiedades antes de conectarse a Birdeye. La lista se ha verificado con [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `WebSocketOrigin` (`string`)
- `Chain` (`string`)
- `TokenAddress` (`string`)
- `StreamingEnabled` (`bool`)
- `PriceInUsd` (`bool`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `MinimumLiquidity` (`decimal`)
- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_birdeye.md)

[Inicialización del adaptador](adapter_initialization_birdeye.md)
