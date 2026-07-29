# Configuración del conector: SEC EDGAR

Configure las siguientes propiedades antes de conectarse a SEC EDGAR. La lista se ha verificado con [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `DataEndpoint` (`Uri`)
- `UserAgent` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, el ritmo de solicitudes, los filtros, las opciones de datos y los límites de resultados.

- `WebsiteEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Forms` (`string`)
- `MaximumHistoricalFiles` (`int`)
- `MaximumFacts` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_sec_edgar.md)

[Inicialización del adaptador](adapter_initialization_sec_edgar.md)
