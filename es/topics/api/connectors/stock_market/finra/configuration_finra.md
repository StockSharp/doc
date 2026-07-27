# Configuración del conector: FINRA

Configure las siguientes propiedades antes de conectarse a FINRA. La lista se ha verificado con [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`FinraDataSets`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Token` (`SecureString`)
- `WeeklyTierIdentifier` (`string`)
- `WeeklySummaryTypeCode` (`string`)
- `PageSize` (`int`)
- `MaxRecords` (`int`)
- `DataVersion` (`int`)
- `Address` (`Uri`)
- `AuthAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_finra.md)

[Inicialización del adaptador](adapter_initialization_finra.md)
