# Configuración del conector: Finage

Configure las siguientes propiedades antes de conectarse a Finage. La lista se ha verificado con [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ApiKey` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan el acceso REST y WebSocket, los puntos de conexión, las opciones de datos, los filtros de símbolos y los límites.

- `StreamingToken` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_finage.md)

[Inicialización del adaptador](adapter_initialization_finage.md)
