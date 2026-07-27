# Configuración del conector: SET Market Data

Configure las siguientes propiedades antes de conectarse a SET Market Data. La lista se ha verificado con [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Address` (`Uri`)
- `DataMode` (`SetMarketDataModes`)
- `Markets` (`string`)
- `IndexSectors` (`string`)
- `SecurityTypeCodes` (`string`)
- `IncludeOddLots` (`bool`)
- `IncludeIndices` (`bool`)

## Véase también

[Configuración gráfica](graphical_configuration_set_market_data.md)

[Inicialización del adaptador](adapter_initialization_set_market_data.md)
