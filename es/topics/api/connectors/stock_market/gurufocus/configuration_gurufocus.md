# Configuración del conector: GuruFocus

Configure las siguientes propiedades antes de conectarse a GuruFocus. La lista se ha verificado con [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RegionCode` (`string`)
- `PageSize` (`int`)
- `MaxLookupPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `FilingFormType` (`string`)
- `GuruTradeActions` (`string`)

## Véase también

[Configuración gráfica](graphical_configuration_gurufocus.md)

[Inicialización del adaptador](adapter_initialization_gurufocus.md)
