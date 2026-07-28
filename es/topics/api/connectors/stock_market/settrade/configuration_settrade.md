# Configuración del conector: Settrade

Configure las siguientes propiedades antes de conectarse a Settrade. La lista se ha verificado con [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AppCode` (`string`)
- `BrokerId` (`string`)
- `Account` (`string`)
- `Pin` (`SecureString`)
- `AccountType` (`SettradeAccountTypes`)
- `IsDemo` (`bool`)

## Configuración avanzada

Estas propiedades controlan los parámetros de acceso, los puntos de conexión de producción y pruebas, y las consultas periódicas del estado privado.

- `LoginParameters` (`string`)
- `RestEndpoint` (`string`)
- `DemoRestEndpoint` (`string`)
- `MarketDataEndpoint` (`string`)
- `DemoMarketDataEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_settrade.md)

[Inicialización del adaptador](adapter_initialization_settrade.md)
