# Configuración del conector: EXANTE

Configure las siguientes propiedades antes de conectarse a EXANTE. La lista se ha verificado con [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `SummaryCurrency` (`string`)
- `PollingInterval` (`TimeSpan`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `MaxMarketDepth` (`int`)
- `HistoryRequestSize` (`int`)
- `LiveAddress` (`Uri`)
- `DemoAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_exante.md)

[Inicialización del adaptador](adapter_initialization_exante.md)
