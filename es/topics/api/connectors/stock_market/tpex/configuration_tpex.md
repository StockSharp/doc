# Configuración del conector: TPEx

Configure las siguientes propiedades antes de conectarse a TPEx. La lista se ha verificado con [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Address` (`Uri`)
- `Market` (`TpexMarkets`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `IncludeListedDerivatives` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)
- `MaxHistoryMonths` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_tpex.md)

[Inicialización del adaptador](adapter_initialization_tpex.md)
