# Configuración del conector: Bavest

Configure las siguientes propiedades antes de conectarse a Bavest. La lista se ha verificado con [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Currency` (`string`)
- `Exchange` (`string`)
- `ExchangeCode` (`string`)
- `FinancialFrequency` (`BavestFinancialFrequencies`)
- `TraceEtfMetrics` (`bool`)
- `ScreenerQuery` (`string`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `NewsLimit` (`int`)
- `DatasetLimit` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_bavest.md)

[Inicialización del adaptador](adapter_initialization_bavest.md)
