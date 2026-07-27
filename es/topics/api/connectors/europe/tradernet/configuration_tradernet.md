# Configuración del conector: Tradernet

Configure las siguientes propiedades antes de conectarse a Tradernet. La lista se ha verificado con [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `PollingInterval` (`TimeSpan`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `MaxMarketDepth` (`int`)
- `SecuritiesPageSize` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_tradernet.md)

[Inicialización del adaptador](adapter_initialization_tradernet.md)
