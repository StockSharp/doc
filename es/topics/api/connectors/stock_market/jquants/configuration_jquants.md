# Configuración del conector: J-Quants

Configure las siguientes propiedades antes de conectarse a J-Quants. La lista se ha verificado con [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, el ritmo de solicitudes, los filtros, las opciones de datos y los límites de resultados.

- `RestEndpoint` (`string`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_jquants.md)

[Inicialización del adaptador](adapter_initialization_jquants.md)
