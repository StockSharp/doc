# Configuración del conector: MarketData.app

Configure las siguientes propiedades antes de conectarse a MarketData.app. La lista se ha verificado con [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `RestEndpoint` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, el ritmo de solicitudes, los filtros, las opciones de datos y los límites de resultados.

- `ExtendedHours` (`bool`)
- `AdjustSplits` (`bool`)
- `MaximumOptionContracts` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_marketdataapp.md)

[Inicialización del adaptador](adapter_initialization_marketdataapp.md)
