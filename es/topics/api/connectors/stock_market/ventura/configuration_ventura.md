# Configuración del conector: Ventura

Configure las siguientes propiedades antes de conectarse a Ventura. La lista se ha verificado con [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ClientId` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RequestToken` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `Pin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `MacAddress` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`VenturaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `OrderStatusAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_ventura.md)

[Inicialización del adaptador](adapter_initialization_ventura.md)
