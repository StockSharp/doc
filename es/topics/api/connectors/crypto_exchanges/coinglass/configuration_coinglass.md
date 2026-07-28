# Configuración del conector: CoinGlass

Configure las siguientes propiedades antes de conectarse a CoinGlass. La lista se ha verificado con [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinGlassMarketTypes`)
- `CandleMetric` (`CoinGlassCandleMetrics`)
- `Exchange` (`string`)
- `Symbol` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_coinglass.md)

[Inicialización del adaptador](adapter_initialization_coinglass.md)
