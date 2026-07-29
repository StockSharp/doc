# Configuración del conector: TraderMade

Configure las siguientes propiedades antes de conectarse a TraderMade. La lista se ha verificado con [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `RestKey` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan el acceso REST y WebSocket, los puntos de conexión, las opciones de datos, los filtros de símbolos y los límites.

- `StreamingKey` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `EnableLadder` (`bool`)
- `Weekend` (`bool`)
- `QuoteCurrencies` (`string`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_tradermade.md)

[Inicialización del adaptador](adapter_initialization_tradermade.md)
