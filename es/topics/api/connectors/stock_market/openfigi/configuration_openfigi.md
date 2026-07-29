# Configuración del conector: OpenFIGI

Configure las siguientes propiedades antes de conectarse a OpenFIGI. La lista se ha verificado con [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, el ritmo de solicitudes, los filtros, las opciones de datos y los límites de resultados.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)
- `MaximumResults` (`int`)
- `ExchangeCode` (`string`)
- `MicCode` (`string`)
- `Currency` (`string`)
- `MarketSector` (`string`)
- `SecurityType2` (`string`)
- `IncludeUnlistedEquities` (`bool`)

## Véase también

[Configuración gráfica](graphical_configuration_openfigi.md)

[Inicialización del adaptador](adapter_initialization_openfigi.md)
