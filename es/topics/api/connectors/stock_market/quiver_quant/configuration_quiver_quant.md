# Configuración del conector: Quiver Quantitative

Configure las siguientes propiedades antes de conectarse a Quiver Quantitative. La lista se ha verificado con [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `LimitInsiderCodes` (`bool`)
- `MostRecentInstitutional` (`bool`)
- `IncludeNewFunds` (`bool`)
- `CorporateDonorCycle` (`string`)

## Véase también

[Configuración gráfica](graphical_configuration_quiver_quant.md)

[Inicialización del adaptador](adapter_initialization_quiver_quant.md)
