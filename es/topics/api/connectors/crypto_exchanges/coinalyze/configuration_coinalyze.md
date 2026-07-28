# Configuración del conector: Coinalyze

Configure las siguientes propiedades antes de conectarse a Coinalyze. La lista se ha verificado con [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinalyzeMarketTypes`)
- `CandleMetric` (`CoinalyzeCandleMetrics`)
- `Exchange` (`string`)
- `ConvertToUsd` (`bool`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RequestInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_coinalyze.md)

[Inicialización del adaptador](adapter_initialization_coinalyze.md)
