# Configuración del conector: Coincall

Configure las siguientes propiedades antes de conectarse a Coincall. La lista se ha verificado con [CoincallMessageAdapter](xref:StockSharp.Coincall.CoincallMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoincallProductTypes`)
- `RestEndpoint` (`string`)
- `OptionsWebSocketEndpoint` (`string`)
- `FuturesWebSocketEndpoint` (`string`)
- `RequestValidityWindow` (`TimeSpan`)
- `PrivatePollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_coincall.md)

[Inicialización del adaptador](adapter_initialization_coincall.md)
