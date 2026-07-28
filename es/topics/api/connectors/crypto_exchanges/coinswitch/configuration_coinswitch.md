# Configuración del conector: CoinSwitch PRO

Configure las siguientes propiedades antes de conectarse a CoinSwitch PRO. La lista se ha verificado con [CoinSwitchMessageAdapter](xref:StockSharp.CoinSwitch.CoinSwitchMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoinSwitchProductTypes`)
- `SpotExchange` (`string`)
- `RestEndpoint` (`string`)
- `HftEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_coinswitch.md)

[Inicialización del adaptador](adapter_initialization_coinswitch.md)
