# Configuración del conector: SimFin

Configure las siguientes propiedades antes de conectarse a SimFin. La lista se ha verificado con [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, el ritmo de solicitudes, los filtros, las opciones de datos y los límites de resultados.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `StatementTypes` (`string`)
- `Period` (`string`)
- `AsReported` (`bool`)
- `IncludeRatios` (`bool`)
- `MaximumRecords` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_simfin.md)

[Inicialización del adaptador](adapter_initialization_simfin.md)
