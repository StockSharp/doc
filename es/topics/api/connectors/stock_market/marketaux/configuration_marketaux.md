# Configuración del conector: Marketaux

Configure las siguientes propiedades antes de conectarse a Marketaux. La lista se ha verificado con [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Languages` (`string`)
- `EntityTypes` (`string`)
- `Countries` (`string`)
- `MustHaveEntities` (`bool`)
- `GroupSimilar` (`bool`)
- `NewsPageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `SentimentInterval` (`MarketauxIntervals`)

## Véase también

[Configuración gráfica](graphical_configuration_marketaux.md)

[Inicialización del adaptador](adapter_initialization_marketaux.md)
