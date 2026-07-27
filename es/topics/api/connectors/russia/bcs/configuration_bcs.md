# Configuración del conector: BCS

Configure las siguientes propiedades antes de conectarse a BCS. La lista se ha verificado con [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `IsReadOnly` (`bool`)
- `PortfolioName` (`string`)
- `PollingInterval` (`TimeSpan`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Véase también

[Configuración gráfica](graphical_configuration_bcs.md)

[Inicialización del adaptador](adapter_initialization_bcs.md)
