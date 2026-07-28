# Configuración del conector: DEX Screener

Configure las siguientes propiedades antes de conectarse a DEX Screener. La lista se ha verificado con [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `RestEndpoint` (`string`)
- `ChainId` (`string`)
- `TokenAddress` (`string`)
- `SearchQuery` (`string`)
- `PriceInUsd` (`bool`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_dex_screener.md)

[Inicialización del adaptador](adapter_initialization_dex_screener.md)
