# Configuración del conector: Korean FSC

Configure las siguientes propiedades antes de conectarse a Korean FSC. La lista se ha verificado con [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `DataSet` (`KoreanFscDataSets`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Address` (`Uri`)
- `Market` (`KoreanFscMarkets`)
- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_korean_fsc.md)

[Inicialización del adaptador](adapter_initialization_korean_fsc.md)
