# Configuración del conector: Directa

Configure las siguientes propiedades antes de conectarse a Directa. La lista se ha verificado con [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Address` (`EndPoint`)
- `DataAddress` (`EndPoint`)
- `HistoryAddress` (`EndPoint`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RequestTimeout` (`TimeSpan`)
- `AutoConfirmOrders` (`bool`)
- `MaxMarketDepth` (`int`)
- `TimeZoneId` (`string`)

## Véase también

[Configuración gráfica](graphical_configuration_directa.md)

[Inicialización del adaptador](adapter_initialization_directa.md)
